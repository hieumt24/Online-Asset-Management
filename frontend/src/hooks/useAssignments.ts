import { AssignmentRes } from "@/models";
import { getAllAssignmentService } from "@/services/admin/manageAssignmentService";
import { useCallback, useEffect, useState } from "react";

export const useAssignments = (
  pagination: {
    pageSize: number;
    pageIndex: number;
  },
  search?: string,
  orderBy?: string,
  adminLocation?: number,
  isDescending?: boolean,
  assignmentState?: number,
  dateFrom?: string,
  dateTo?: string
) => {
  const [assignments, setAssignments] = useState<AssignmentRes[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<boolean | null>(false);
  const [pageCount, setPageCount] = useState<number>(0);
  const [totalRecords, setTotalRecords] = useState<number>(0);

  // Function to fetch assignments
  const fetchAssignments = useCallback(async (isEdited?: string | null) => {
    setLoading(true);
    try {
      isEdited && (pagination.pageSize -= 1)
      const data = await getAllAssignmentService({
        pagination,
        search,
        orderBy,
        isDescending,
        adminLocation,
        assignmentState,
        dateFrom,
        dateTo
      });

      setAssignments(data.data.data || []);
      setPageCount(data.data.totalPages);
      setTotalRecords(data.data.totalRecords);
      isEdited && (pagination.pageSize += 1)
      return data.data.data || [];
    } catch (error) {
      console.error("Error in fetchAssignments:", error);
      setError(true);
      return [];
    } finally {
      setLoading(false);
    }
  }, [
    pagination,
    search,
    orderBy,
    isDescending,
    adminLocation,
    assignmentState,
    dateFrom,
    dateTo
  ]);

  useEffect(() => {
    const fetchAndUpdateAssignments = async () => {
      if (!dateTo && dateFrom) return;
      setLoading(true);
      try {
        const isEdited = localStorage.getItem("edited");

        // Always fetch the latest data
        const currentAssignments = await fetchAssignments(isEdited);

        if (isEdited) {
          const orderByField = isEdited
              ? "lastModifiedOn"
              : orderBy;
          const newAssignmentData = await getAllAssignmentService({
            pagination,
            search,
            orderBy: orderByField,
            isDescending: true, // Ensure we get the most recent assignment
            adminLocation,
            assignmentState,
            dateFrom,
            dateTo
          });

          const newAssignment = newAssignmentData.data.data[0];
          if (newAssignment) {
            setAssignments([
              newAssignment,
              ...currentAssignments.filter(
                (assignment: AssignmentRes) =>
                  assignment.id !== newAssignment.id,
              ),
            ]);
          }

          localStorage.removeItem("edited");
        }
      } catch (error) {
        console.error("Error in fetchAndUpdateAssignments:", error);
        setError(true);
      } finally {
        setLoading(false);
      }
    };

    fetchAndUpdateAssignments();
  }, [
    pagination,
    search,
    orderBy,
    isDescending,
    adminLocation,
    assignmentState,
    dateFrom,
    dateTo,
    fetchAssignments,
  ]);

  return {
    assignments,
    loading,
    error,
    setAssignments,
    pageCount,
    totalRecords,
    fetchAssignments,
  };
};
