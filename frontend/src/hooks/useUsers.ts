import { UserRes } from "@/models";
import { getAllUserService } from "@/services";
import { useCallback, useEffect, useState } from "react";

export const useUsers = (
  pagination: {
    pageIndex: number;
    pageSize: number;
  },
  adminLocation: number,
  search?: string,
  roleType?: number,
  orderBy?: string,
  isDescending?: boolean,
) => {
  const [users, setUsers] = useState<UserRes[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<boolean | null>(false);
  const [pageCount, setPageCount] = useState<number>(0);
  const [totalRecords, setTotalRecords] = useState<number>(0);

  const fetchUsers = useCallback(async (isEdited?: string | null) => {
    setLoading(true);
    try {
      isEdited && (pagination.pageSize -= 1)
      const data = await getAllUserService({
        pagination,
        search,
        roleType,
        adminLocation,
        orderBy,
        isDescending,
      });

      setUsers(data.data.data || []);
      setPageCount(data.data.totalPages);
      setTotalRecords(data.data.totalRecords);
      isEdited && (pagination.pageSize += 1)
      return data.data.data || [];
    } catch (error) {
      console.error("Error in fetchUsers:", error);
      setError(true);
      return [];
    } finally {
      setLoading(false);
    }
  }, [pagination, search, roleType, adminLocation, orderBy, isDescending]);

  useEffect(() => {
    const fetchAndUpdateUsers = async () => {
      setLoading(true);
      try {
        const isEdited = localStorage.getItem("edited");

        // Always fetch the latest data
        const currentUsers = await fetchUsers(isEdited);

        if (isEdited) {
          const newUserData = await getAllUserService({
            pagination,
            search,
            roleType,
            adminLocation,
            orderBy: isEdited
                ? "lastModifiedOn"
                : orderBy,
            isDescending: true, // Ensure we get the most recent user
          });

          const newUser = newUserData.data.data[0];
          if (newUser) {
            setUsers([
              newUser,
              ...currentUsers.filter((user: UserRes) => user.id !== newUser.id),
            ]);
          }

          localStorage.removeItem("edited");
        }
      } catch (error) {
        console.error("Error in fetchAndUpdateUsers:", error);
        setError(true);
      } finally {
        setLoading(false);
      }
    };

    fetchAndUpdateUsers();
  }, [
    pagination,
    search,
    roleType,
    adminLocation,
    orderBy,
    isDescending,
    fetchUsers,
  ]);

  return {
    users,
    loading,
    error,
    setUsers,
    pageCount,
    totalRecords,
    fetchUsers,
  };
};
