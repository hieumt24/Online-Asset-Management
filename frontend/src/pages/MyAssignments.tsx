import { DateRangePicker, GenericDialog, SearchForm } from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { MyAssignmentTable } from "@/components/tables/assignment/MyAssignmentTable";
import { myAssignmentColumns } from "@/components/tables/assignment/myAssignmentColumns";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useLoading } from "@/context/LoadingContext";
import { useAuth, usePagination } from "@/hooks";
import { useMyAssignments } from "@/hooks/useMyAssignments";
import { updateAssignmentStateService } from "@/services/admin/manageAssignmentService";
import { createReturnRequest } from "@/services/admin/manageReturningRequestService";
import { format } from "date-fns";
import { useState } from "react";
import { DateRange } from "react-day-picker";
import { toast } from "sonner";

export const MyAssignment = () => {
  const { onPaginationChange, pagination } = usePagination();
  const [search, setSearch] = useState("");
  const [orderBy, setOrderBy] = useState("");
  const [isDescending, setIsDescending] = useState(true);
  const [assignmentState, setAssignmentState] = useState(0);
  const [assignedDate, setAssignedDate] = useState<DateRange | null>(null);
  const { user } = useAuth();

  const {
    assignments,
    loading,
    error,
    pageCount,
    totalRecords,
    fetchAssignments,
  } = useMyAssignments(
    pagination,
    user.id,
    search.trim(),
    orderBy,
    isDescending,
    assignmentState,
    assignedDate?.from ? format(assignedDate.from, "yyyy-MM-dd") : "",
    assignedDate?.to ? format(assignedDate.to, "yyyy-MM-dd") : "",
  );

  const { setIsLoading } = useLoading();
  const [assignmentId, setAssignmentId] = useState<string>("");
  const [openDecline, setOpenDecline] = useState(false);
  const [openCreateRequest, setOpenCreateRequest] = useState(false);
  const [openAccept, setOpenAccept] = useState(false);

  const handleOpenDecline = (id: string) => {
    setAssignmentId(id);
    setOpenDecline(true);
  };
  const handleOpenCreateRequest = (id: string) => {
    setAssignmentId(id);
    setOpenCreateRequest(true);
  };

  const handleOpenAccept = (id: string) => {
    setAssignmentId(id);
    setOpenAccept(true);
  };

  const handleDecline = async () => {
    setIsLoading(true);
    const res = await updateAssignmentStateService({
      assignmentId: assignmentId,
      assignedIdTo: user.id,
      newState: 3,
    });
    if (res.success) {
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    fetchAssignments();
    setOpenDecline(false);
    setIsLoading(false);
  };

  const handleCreateRequest = async () => {
    setIsLoading(true);
    const res = await createReturnRequest({
      assignmentId: assignmentId,
      requestedBy: user.id,
      location: user.location,
    });
    if (res.success) {
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    fetchAssignments();
    setOpenCreateRequest(false);
    setIsLoading(false);
  };

  const handleAccept = async () => {
    setIsLoading(true);
    const res = await updateAssignmentStateService({
      assignmentId: assignmentId,
      assignedIdTo: user.id,
      newState: 1,
    });
    if (res.success) {
      toast.success(res.message);
    } else {
      toast.error(res.message);
    }
    fetchAssignments();
    setOpenAccept(false);
    setIsLoading(false);
  };

  return (
    <div className="m-16 flex h-full flex-grow flex-col gap-8">
      <p className="text-2xl font-bold text-red-600">My Assignment</p>
      <div className="flex items-center justify-between">
        <div className="flex items-center justify-center gap-2">
          <Select
            onValueChange={(value) => {
              pagination.pageIndex = 1;
              setAssignmentState(Number(value));
            }}
          >
            <SelectTrigger className="min-w-24">
              <SelectValue placeholder="State" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="0">All States</SelectItem>
              <SelectItem value="1">Accepted</SelectItem>
              <SelectItem value="2">Waiting for acceptance</SelectItem>
              <SelectItem value="4">Waiting for returning</SelectItem>
            </SelectContent>
          </Select>
          <div className="flex items-center">
            <DateRangePicker
              setValue={setAssignedDate}
              placeholder="Assigned Date"
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
            placeholder="Asset code, asset name, assigner"
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
          <MyAssignmentTable
            columns={myAssignmentColumns({
              handleOpenCreateRequest,
              handleOpenAccept,
              handleOpenDecline,
              setOrderBy,
              setIsDescending,
              isDescending,
              orderBy,
            })}
            data={assignments!}
            pagination={pagination}
            onPaginationChange={onPaginationChange}
            pageCount={pageCount}
            totalRecords={totalRecords}
          />
          <GenericDialog
            title="Are you sure?"
            desc="Do you want to decline this assignment"
            confirmText="Decline"
            cancelText="Cancel"
            onConfirm={handleDecline}
            open={openDecline}
            setOpen={setOpenDecline}
          />
          <GenericDialog
            title="Are you sure?"
            desc="Do you want to create a returning request for this asset?"
            confirmText="Yes"
            cancelText="No"
            open={openCreateRequest}
            setOpen={setOpenCreateRequest}
            onConfirm={handleCreateRequest}
          />
          <GenericDialog
            title="Are you sure?"
            desc="Do you want to accept this assignment?"
            confirmText="Accept"
            cancelText="Cancel"
            open={openAccept}
            setOpen={setOpenAccept}
            onConfirm={handleAccept}
          />
        </>
      )}
    </div>
  );
};
