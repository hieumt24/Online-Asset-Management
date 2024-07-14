import { DateRangePicker, GenericDialog, SearchForm } from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

import { ReturningRequestTable } from "@/components/tables/returningRequest/ReturningRequestTable";
import { returningRequestColumns } from "@/components/tables/returningRequest/returningRequestColumns";
import { useLoading } from "@/context/LoadingContext";
import { useAuth } from "@/hooks";
import { usePagination } from "@/hooks/usePagination";
import { useReturningRequests } from "@/hooks/useReturningRequests";
import {
  cancelReturnRequest,
  updateReturnRequest,
} from "@/services/admin/manageReturningRequestService";
import { format } from "date-fns";
import { useState } from "react";
import { DateRange } from "react-day-picker";
import { toast } from "sonner";

export const ManageReturningRequest = () => {
  const { user } = useAuth();
  const { onPaginationChange, pagination } = usePagination();
  const [search, setSearch] = useState("");

  const [orderBy, setOrderBy] = useState("");
  const [isDescending, setIsDescending] = useState(true);
  const [requestState, setRequestState] = useState(0);
  const [returnedDate, setReturnedDate] = useState<DateRange | null>(null);
  const { requests, loading, error, pageCount, fetchRequests, totalRecords } =
    useReturningRequests(
      pagination,
      user.location,
      requestState,
      returnedDate?.from ? format(returnedDate?.from, "yyyy-MM-dd") : "",
      returnedDate?.to ? format(returnedDate?.to, "yyyy-MM-dd") : "",
      search.trim(),
      orderBy,
      isDescending,
    );
  const [requestId, setRequestId] = useState<string>("");
  const handleOpenCancel = (id: string) => {
    setRequestId(id);
    setOpenCancel(true);
  };
  const handleOpenComplete = (id: string) => {
    setRequestId(id);
    setOpenComplete(true);
  };

  const { setIsLoading } = useLoading();

  const handleCancel = async () => {
    setIsLoading(true);
    const res = await cancelReturnRequest(requestId);
    if (res.success) {
      toast.success(res.message);
      fetchRequests();
    } else {
      toast.error(res.message);
    }
    setOpenCancel(false);
    setIsLoading(false);
  };
  const handleComplete = async () => {
    setIsLoading(true);
    const res = await updateReturnRequest({
      returnRequestId: requestId,
      newState: 2,
      acceptedBy: user.id,
    });
    if (res.success) {
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    fetchRequests();
    setOpenComplete(false);
    setIsLoading(false);
  };

  const handleValueChange = (value: any) => {
    setRequestState(parseInt(value));
    pagination.pageIndex = 1;
  };
  const [openCancel, setOpenCancel] = useState(false);

  const [openComplete, setOpenComplete] = useState(false);

  return (
    <div className="m-16 flex flex-grow flex-col gap-8">
      <p className="text-2xl font-bold text-red-600">Request List</p>
      <div className="flex items-center justify-between">
        <div className="flex items-center justify-center gap-4">
          <Select onValueChange={handleValueChange}>
            <SelectTrigger className="min-w-24">
              <SelectValue placeholder="State" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="0">All States</SelectItem>
              <SelectItem value="1">Waiting for returning</SelectItem>
              <SelectItem value="2">Completed</SelectItem>
              {/* <SelectItem value="3">Cancelled</SelectItem> */}
            </SelectContent>
          </Select>
          <div className="flex items-center">
            <DateRangePicker
              setValue={setReturnedDate}
              placeholder="Returned Date"
              className="min-w-[150px]"
            />
          </div>
        </div>

        <div className="flex justify-between gap-6">
          <SearchForm
            setSearch={setSearch}
            onSubmit={() => {
              onPaginationChange((prev) => ({
                ...prev,
                pageIndex: 1,
              }));
            }}
            placeholder="Asset code, asset name, requested by"
            className="w-[350px]"
          />
        </div>
      </div>
      {loading ? (
        <LoadingSpinner />
      ) : error ? (
        <div>Error</div>
      ) : (
        <>
          <ReturningRequestTable
            columns={returningRequestColumns({
              handleOpenComplete,
              handleOpenCancel,
              setOrderBy,
              setIsDescending,
              isDescending,
              orderBy,
            })}
            data={requests!}
            onPaginationChange={onPaginationChange}
            pagination={pagination}
            pageCount={pageCount}
            totalRecords={totalRecords}
          />

          <GenericDialog
            title="Are you sure?"
            desc="Do you want to cancel this request?"
            confirmText="Yes"
            cancelText="No"
            open={openCancel}
            setOpen={setOpenCancel}
            onConfirm={handleCancel}
          />
          <GenericDialog
            title="Are you sure?"
            desc="Do you want to mark this returning as 'Completed'?"
            confirmText="Yes"
            cancelText="No"
            open={openComplete}
            setOpen={setOpenComplete}
            onConfirm={handleComplete}
          />
        </>
      )}
    </div>
  );
};
