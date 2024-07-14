import { ReportTable, SearchForm, reportColumns } from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { Button } from "@/components/ui/button";

import { useAuth, useReport } from "@/hooks";
import { usePagination } from "@/hooks/usePagination";
import { exportReportService } from "@/services";
import { useState } from "react";

export const Report = () => {
  const { user } = useAuth();
  const { onPaginationChange, pagination } = usePagination();
  const [search, setSearch] = useState("");

  const [orderBy, setOrderBy] = useState("");
  const [isDescending, setIsDescending] = useState(true);
  const { report, loading, error, pageCount, totalRecords } = useReport({
    pagination,
    search,
    location: user.location,
    orderBy,
    isDescending,
  });


  return (
    <div className="m-16 flex flex-grow flex-col gap-8">
      <p className="text-2xl font-bold text-red-600">Report</p>
      <div className="flex items-center justify-end gap-6">
        <SearchForm
          setSearch={setSearch}
          onSubmit={() => {
            onPaginationChange((prev) => ({
              ...prev,
              pageIndex: 1,
            }));
          }}
          placeholder="Category"
          className="w-[300px]"
        />
        <Button
          variant={"destructive"}
          onClick={() => exportReportService(user.location)}
        >
          <span className="capitalize">Export</span>
        </Button>
      </div>
      {loading ? (
        <LoadingSpinner />
      ) : error ? (
        <div>Error</div>
      ) : (
        <ReportTable
          columns={reportColumns({
            setOrderBy,
            setIsDescending,
            isDescending,
            orderBy,
          })}
          data={report!}
          onPaginationChange={onPaginationChange}
          pagination={pagination}
          pageCount={pageCount}
          totalRecords={totalRecords}
        />
      )}
    </div>
  );
};
