import { Input } from "@/components/ui/input";
import { renderHeader } from "@/lib/utils";
import { AssetRes } from "@/models";
import { ColumnDef } from "@tanstack/react-table";

interface AssignmentAssetColumnsProps {
  selectedId: string;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>;
  isDescending: boolean;
  orderBy: string;
}

export const assignmentAssetColumns = ({
  selectedId,
  setOrderBy,
  setIsDescending,
  isDescending,
  orderBy,
}: AssignmentAssetColumnsProps): ColumnDef<AssetRes>[] => [
  {
    accessorKey: "id",
    header: "",
    cell: ({ row }) => {
      return (
        <Input
          className="w-4 shadow-none"
          type="radio"
          checked={selectedId === row.original.id}
          name={"assignment-asset-select"}
          readOnly
        />
      );
    },
  },
  {
    accessorKey: "assetCode",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "assetName",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "categoryName",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
];
