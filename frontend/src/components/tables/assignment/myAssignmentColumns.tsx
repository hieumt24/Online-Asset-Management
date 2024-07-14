import { renderHeader } from "@/lib/utils";
import { AssignmentRes } from "@/models";
import { ColumnDef } from "@tanstack/react-table";
import { format } from "date-fns";
import { FaCheck } from "react-icons/fa";
import { IoCloseCircleOutline, IoReload } from "react-icons/io5";

interface AssignmentColumnsProps {
  handleOpenCreateRequest: (id: string) => void;
  handleOpenAccept: (id: string) => void;
  handleOpenDecline: (id: string) => void;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>;
  isDescending: boolean;
  orderBy: string;
}

export const myAssignmentColumns = ({
  handleOpenCreateRequest,
  handleOpenAccept,
  handleOpenDecline,
  setOrderBy,
  setIsDescending,
  isDescending,
  orderBy,
}: AssignmentColumnsProps): ColumnDef<AssignmentRes>[] => [
  {
    accessorKey: "id",
    header: "",
    cell: ({ row }) => {
      const assignment = row.original;
      return <div className="hidden">{assignment.id}</div>;
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
    accessorKey: "assignedBy",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "assignedDate",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
    cell: ({ row }) => {
      const formattedDate = format(row.original.assignedDate!, "dd/MM/yyyy");
      return <p>{formattedDate}</p>;
    },
  },
  {
    accessorKey: "state",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
    cell: ({ row }) => {
      const state = row.original.state;
      switch (state) {
        case 1:
          return <p className="text-green-600">Accepted</p>;
        case 2:
          return <p className="text-yellow-600">Waiting for acceptance</p>;
        case 3:
          return <p className="text-red-600">Declined</p>;
        case 4:
          return <p className="text-blue-600">Waiting for returning</p>;

        default:
          return <p>Not found state</p>;
      }
    },
  },
  {
    accessorKey: "action",
    header: "Actions",
    cell: ({ row }) => {
      const assignment = row.original;
      return (
        <div className="flex gap-3">
          <button
            className="text-blue-500 hover:text-blue-700"
            onClick={(e) => {
              e.stopPropagation();
              handleOpenAccept(assignment.id!);
            }}
            disabled={assignment.state !== 2}
            title="Accept Assignment"
          >
            {assignment.state !== 2 ? (
              <FaCheck
                size={18}
                className="text-black opacity-25 hover:text-black"
              />
            ) : (
              <FaCheck size={18} />
            )}
          </button>
          <button
            className="text-red-500 hover:text-red-700"
            onClick={(e) => {
              e.stopPropagation();
              handleOpenDecline(assignment.id!);
            }}
            disabled={assignment.state !== 2}
            title="Decline Assignment"
          >
            {assignment.state !== 2 ? (
              <IoCloseCircleOutline
                size={20}
                className="text-black opacity-25 hover:text-black"
              />
            ) : (
              <IoCloseCircleOutline size={20} />
            )}
          </button>
          <button
            className="text-green-500 hover:text-green-700"
            onClick={(e) => {
              e.stopPropagation();
              handleOpenCreateRequest(assignment.id!);
            }}
            disabled={assignment.state !== 1}
            title="Create Return Request"
          >
            {assignment.state !== 1 ? (
              <IoReload
                size={20}
                className="text-black opacity-25 hover:text-black"
              />
            ) : (
              <IoReload size={20} />
            )}
          </button>
        </div>
      );
    },
  },
];
