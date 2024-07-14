import { CaretSortIcon } from "@radix-ui/react-icons";
import React from "react";
import { MdKeyboardArrowLeft, MdKeyboardArrowRight } from "react-icons/md";
import { Button } from "../ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../ui/select";

interface PaginationProps {
  pageIndex: number;
  pageCount: number;
  pageSize: number;
  totalRecords: number;
  setPage: (pageIndex: number) => void;
  adjustablePageSize?: boolean;
  setPageSize?: (
    pageSize: string,
  ) => void | React.Dispatch<React.SetStateAction<string>>;
}
const Pagination: React.FC<PaginationProps> = ({
  pageIndex,
  pageCount,
  pageSize,
  setPage,
  totalRecords = 0,
  adjustablePageSize = true,
  setPageSize,
}) => {
  const getPaginationNumbers = () => {
    const pageNumbers: (number | string)[] = [];

    if (pageCount <= 7) {
      for (let i = 1; i <= pageCount; i++) {
        pageNumbers.push(i);
      }
    } else {
      pageNumbers.push(1); // Always include the first page

      if (pageIndex > 3) {
        pageNumbers.push("...");
      }

      for (
        let i = Math.max(2, pageIndex - 1);
        i <= Math.min(pageIndex + 1, pageCount - 1);
        i++
      ) {
        pageNumbers.push(i);
      }

      if (pageIndex < pageCount - 2) {
        pageNumbers.push("...");
      }

      pageNumbers.push(pageCount); // Always include the last page
    }

    return pageNumbers;
  };

  return (
    <div className="flex items-center justify-between space-x-2 py-4">
      {adjustablePageSize && (<div className="flex items-center gap-2">
        <div className="text-sm">Records per page</div>
        <Select onValueChange={setPageSize} value={pageSize.toString()}>
          <SelectTrigger className="w-[80px]" icon={<CaretSortIcon className="ml-3 h-4 w-4 opacity-50" />}>
            <SelectValue placeholder="Per page" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="10">10</SelectItem>
            <SelectItem value="15">15</SelectItem>
            <SelectItem value="20">20</SelectItem>
          </SelectContent>
        </Select>
      </div>)}
      <div className="flex items-center justify-end space-x-2 flex-grow">
        {totalRecords !== 0 && (
          <div className="text-sm">
            Showing {(pageIndex - 1) * pageSize + 1} -{" "}
            {Math.min(pageIndex * pageSize, totalRecords)} of {totalRecords}{" "}
            records
          </div>
        )}
        {pageCount > 1 && (
          <div className="flex space-x-2 items-center">
            <Button
              variant="destructive"
              size="sm"
              onClick={() => setPage(--pageIndex)}
              disabled={pageIndex === 1}
              className="px-1"
            >
              <MdKeyboardArrowLeft size={24} />
            </Button>
            {getPaginationNumbers().map((page, idx) => (
              <button
                key={idx}
                className={`rounded-md border px-3 py-1 transition-all ${
                  page === pageIndex
                    ? "bg-red-600 text-white"
                    : "border-gray-300"
                } ${typeof page === "number" ? "hover:bg-red-300" : "cursor-default"}`}
                onClick={() => {
                  typeof page === "number" && setPage(page);
                }}
                disabled={typeof page !== "number"}
              >
                {page}
              </button>
            ))}
            <Button
              variant="destructive"
              size="sm"
              onClick={() => setPage(++pageIndex)}
              disabled={pageIndex === pageCount}
              className="px-1"
            >
              <MdKeyboardArrowRight size={24} />
            </Button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Pagination;
