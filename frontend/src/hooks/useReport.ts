import { GetReportReq, ReportRes } from "@/models";
import { getReportService } from "@/services";
import { useCallback, useEffect, useState } from "react";

export const useReport = ({
  pagination,
  search,
  location,
  orderBy,
  isDescending,
}: GetReportReq) => {
  const [report, setReport] = useState<ReportRes[] | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<boolean | null>(false);
  const [pageCount, setPageCount] = useState<number>(0);
  const [totalRecords, setTotalRecords] = useState<number>(0);

  const fetchReport = useCallback(async () => {
    setLoading(true);
    try {
      const data = await getReportService({
        pagination,
        search,
        location,
        orderBy,
        isDescending,
      });

      setReport(data.data.data || []);
      setPageCount(data.data.totalPages);
      setTotalRecords(data.data.totalRecords);
      return data.data.data || [];
    } catch (error) {
      console.error("Error in fetchReport:", error);
      setError(true);
      return [];
    } finally {
      setLoading(false);
    }
  }, [pagination, search, location, orderBy, isDescending]);

  useEffect(() => {
    fetchReport();
  }, [pagination, search, location, orderBy, isDescending, fetchReport]);

  return {
    report,
    loading,
    error,
    setReport,
    pageCount,
    totalRecords,
    fetchReport,
  };
};
