import { renderHeader } from "@/lib/utils";
import { ReportRes } from "@/models";
import { ColumnDef } from "@tanstack/react-table";

interface ReportColumnsProps {
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>;
  isDescending: boolean;
  orderBy: string;
}

export const reportColumns = ({
  setOrderBy,
  setIsDescending,
  isDescending,
  orderBy,
}: ReportColumnsProps): ColumnDef<ReportRes>[] => [
  {
    accessorKey: "categoryName",
    header: ({ column }) =>
      renderHeader(
        column,
        setOrderBy,
        setIsDescending,
        isDescending,
        orderBy,
        "Category",
      ),
  },
  {
    accessorKey: "total",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "assigned",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "available",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "notAvailable",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "waitingForRecycling",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "recycled",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
];
