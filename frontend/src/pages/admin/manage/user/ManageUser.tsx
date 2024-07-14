import {
  GenericDialog,
  SearchForm,
  UserTable,
  userColumns,
} from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

import { useLoading } from "@/context/LoadingContext";
import { useAuth, useUsers } from "@/hooks";
import { usePagination } from "@/hooks/usePagination";
import { checkUserHasAssignmentService, disableUserService } from "@/services";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";

export const ManageUser = () => {
  const { user } = useAuth();
  const { onPaginationChange, pagination } = usePagination();
  const [search, setSearch] = useState("");

  const [orderBy, setOrderBy] = useState("");
  const [isDescending, setIsDescending] = useState(true);
  const [roleType, setRoleType] = useState(0);
  const { users, loading, error, pageCount, fetchUsers, totalRecords } =
    useUsers(
      pagination,
      user.location,
      search.trim(),
      roleType,
      orderBy,
      isDescending,
    );

  const [openDisable, setOpenDisable] = useState(false);
  const [openCannotDisable, setOpenCannotDisable] = useState(false);

  const navigate = useNavigate();
  const { setIsLoading } = useLoading();
  const [userIdToDisable, setUserIdToDisable] = useState<string>("");
  const handleOpenDisable = async (id: string) => {
    const result = await checkUserHasAssignmentService(id);
    if (result.success) {
      setUserIdToDisable(id);
      setOpenDisable(true);
    } else {
      setOpenCannotDisable(true);
    }
  };

  const handleDisable = async () => {
    try {
      setIsLoading(true);
      const res = await disableUserService(userIdToDisable);
      if (res.success) {
        toast.success(res.message);
      } else {
        toast.error(res.message);
      }
      fetchUsers();
      setOpenDisable(false);
    } catch (err) {
      console.log(err);
      toast.error("Error when disable user");
    } finally {
      setIsLoading(false);
    }
  };

  const handleValueChange = (value: string) => {
    setRoleType(parseInt(value));
    pagination.pageIndex = 1;
  };

  return (
    <div className="m-16 flex flex-grow flex-col gap-8">
      <div className="flex justify-between">
        <p className="text-2xl font-bold text-red-600">User List</p>
        <Button
          variant={"destructive"}
          onClick={() => navigate("/users/create")}
        >
          <span className="capitalize">Create new user</span>
        </Button>
      </div>

      <div className="flex items-center justify-between">
        <Select onValueChange={handleValueChange}>
          <SelectTrigger className="w-32">
            <SelectValue placeholder="Role" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="0">All roles</SelectItem>
            <SelectItem value="1">Admin</SelectItem>
            <SelectItem value="2">Staff</SelectItem>
          </SelectContent>
        </Select>

        <div className="flex justify-between gap-6">
          <SearchForm
            setSearch={setSearch}
            onSubmit={() => {
              onPaginationChange((prev) => ({
                ...prev,
                pageIndex: 1,
              }));
            }}
            placeholder="Staff code, username, full name"
            className="w-[300px]"
          />
        </div>
      </div>
      {loading ? (
        <LoadingSpinner />
      ) : error ? (
        <div>Error</div>
      ) : (
        <>
          <UserTable
            columns={userColumns({
              handleOpenDisable,
              setOrderBy,
              setIsDescending,
              isDescending,
              orderBy,
            })}
            data={users!}
            onPaginationChange={onPaginationChange}
            pagination={pagination}
            pageCount={pageCount}
            totalRecords={totalRecords}
          />
          <GenericDialog
            title="Are you sure?"
            desc="Do you want to disable this user?"
            confirmText="Disable"
            cancelText="Cancel"
            open={openDisable}
            setOpen={setOpenDisable}
            onConfirm={handleDisable}
          />
          <GenericDialog
            title="Cannot disable user"
            desc="There are valid assignments belong to this user. Please close all assignments before disabling user."
            open={openCannotDisable}
            setOpen={setOpenCannotDisable}
          />
        </>
      )}
    </div>
  );
};
