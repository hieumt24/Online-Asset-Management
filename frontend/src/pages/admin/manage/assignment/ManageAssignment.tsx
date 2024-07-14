import {
  DateRangePicker,
  GenericDialog,
  SearchForm,
  assignmentColumns,
} from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { AssignmentTable } from "@/components/tables/assignment/AssignmentTable";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useLoading } from "@/context/LoadingContext";
import { useAssignments, useAuth, usePagination } from "@/hooks";
import { deleteAssignmentService } from "@/services/admin/manageAssignmentService";
import { createReturnRequest } from "@/services/admin/manageReturningRequestService";
import { format } from "date-fns";
import { useEffect, useState } from "react";
import { DateRange } from "react-day-picker";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";

export const ManageAssignment = () => {
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
  } = useAssignments(
    pagination,
    search.trim(),
    orderBy,
    user.location,
    isDescending,
    assignmentState,
    assignedDate?.from ? format(assignedDate.from, "yyyy-MM-dd") : "",
    assignedDate?.to ? format(assignedDate.to, "yyyy-MM-dd") : "",
  );

  const { setIsLoading } = useLoading();
  const [openDelete, setOpenDelete] = useState(false);
  const [assignmentId, setAssignmentId] = useState<string>("");
  const [openCreateRequest, setOpenCreateRequest] = useState(false);
  const handleOpenDelete = (id: string) => {
    setAssignmentId(id);
    setOpenDelete(true);
  };
  const handleOpenCreateRequest = (id: string) => {
    setAssignmentId(id);
    setOpenCreateRequest(true);
  };

  const handleDelete = async () => {
    setIsLoading(true);
    const res = await deleteAssignmentService(assignmentId);
    if (res.success) {
      toast.success(res.message);
      fetchAssignments();
    } else {
      toast.error(res.message);
    }
    setOpenDelete(false);
    setIsLoading(false);
    fetchAssignments();
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

  const handleValueChange = (value: any) => {
    setAssignmentState(Number(value));
    pagination.pageIndex = 1;
  };

  useEffect(() => {
    if (assignedDate?.from && assignedDate?.to) {
      pagination.pageIndex = 1;
    }
  }, [assignedDate]);

  const navigate = useNavigate();
  return (
    <div className="m-16 flex flex-grow flex-col gap-8">
      <div className="flex items-center justify-between">
        <p className="text-2xl font-bold text-red-600">Assignment List</p>
        <Button
          variant={"destructive"}
          onClick={() => navigate("/assignments/create")}
        >
          <span className="capitalize">Create new assignment</span>
        </Button>
      </div>

      <div className="flex items-center justify-between gap-4">
        <div className="flex items-center justify-center gap-2">
          <Select onValueChange={handleValueChange}>
            <SelectTrigger className="min-w-24">
              <SelectValue placeholder="State" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="0">All states</SelectItem>
              <SelectItem value="1">Accepted</SelectItem>
              <SelectItem value="2">Waiting for acceptance</SelectItem>
              <SelectItem value="3">Declined</SelectItem>
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
        <div className="flex justify-between gap-4">
          <SearchForm
            setSearch={setSearch}
            onSubmit={() => {
              onPaginationChange((prev) => ({
                ...prev,
                pageIndex: 1,
              }));
            }}
            placeholder="Asset code, asset name, assignee, assigner"
            className="w-[352px]"
          />
        </div>
      </div>

      {loading ? (
        <LoadingSpinner />
      ) : error ? (
        <div>Error</div>
      ) : (
        <>
          <AssignmentTable
            columns={assignmentColumns({
              handleOpenCreateRequest,
              handleOpenDelete,
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
            desc="Do you want to delete this assignment?"
            confirmText="Delete"
            cancelText="Cancel"
            onConfirm={handleDelete}
            open={openDelete}
            setOpen={setOpenDelete}
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
        </>
      )}
    </div>
  );
};
