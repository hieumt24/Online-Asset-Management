import { Input } from "@/components/ui/input";
import { renderHeader } from "@/lib/utils";
import { UserRes } from "@/models";
import { ColumnDef } from "@tanstack/react-table";

interface AssignmentUserColumnsProps {
  selectedId: string;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>;
  isDescending: boolean;
  orderBy: string;
}

export const assignmentUserColumns = ({
  selectedId,
  setOrderBy,
  setIsDescending,
  isDescending,
  orderBy,
}: AssignmentUserColumnsProps): ColumnDef<UserRes>[] => [
  {
    accessorKey: "id",
    header: "",
    cell: ({ row }) => {
      return (
        <Input
          className="w-4 shadow-none"
          type="radio"
          checked={selectedId === row.original.id}
          name={"assignment-user-select"}
          readOnly
        />
      );
    },
  },
  {
    accessorKey: "staffCode",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "username",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "fullName",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
    cell: ({ row }) => {
      const user = row.original;
      return <p>{`${user.firstName} ${user.lastName}`}</p>;
    },
  },
  {
    accessorKey: "role",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
    cell: ({ row }) => {
      const role = row.original.role;
      return <p>{role === 1 ? "Admin" : "Staff"}</p>;
    },
  },
];
