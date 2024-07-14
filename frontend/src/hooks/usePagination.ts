import { useState } from "react";

export const usePagination = () => {
  const [pagination, setPagination] = useState({
    pageSize: 10,
    pageIndex: 1,
  });

  const { pageIndex, pageSize } = pagination;

  return {
    pageSize,
    pageIndex,
    onPaginationChange: setPagination,
    pagination,
  };
};
