import { renderHeader } from "@/lib/utils";
import { AssignmentRes } from "@/models";
import { ColumnDef } from "@tanstack/react-table";
import { format } from "date-fns";
import { FiEdit2 } from "react-icons/fi";
import { IoCloseCircleOutline, IoReload } from "react-icons/io5";
import { useNavigate } from "react-router-dom";

interface AssignmentColumnsProps {
  handleOpenCreateRequest?: (id: string) => void;
  handleOpenDelete?: (id: string) => void;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>;
  isDescending: boolean;
  orderBy: string;
}

export const assignmentColumns = ({
  handleOpenCreateRequest,
  handleOpenDelete,
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
    accessorKey: "assignedTo",
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
        case 5:
          return <p className="text-red-600">Returned</p>;
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
      // eslint-disable-next-line react-hooks/rules-of-hooks
      const navigate = useNavigate();
      return (
        <div className="flex gap-3">
          <button
            className="text-blue-500 hover:text-blue-700"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/assignments/edit/${assignment.id}`);
            }}
            disabled={assignment.state !== 2}
            title="Edit Assignment"
          >
            {assignment.state !== 2 ? (
              <FiEdit2
                size={18}
                className="text-black opacity-25 hover:text-black"
              />
            ) : (
              <FiEdit2 size={18} />
            )}
          </button>
          <button
            className="text-red-500 hover:text-red-700"
            onClick={(e) => {
              e.stopPropagation();
              handleOpenDelete && handleOpenDelete(assignment.id!);
            }}
            disabled={assignment.state === 1 || assignment.state === 4}
            title="Delete Assignment"
          >
            {assignment.state === 1 || assignment.state === 4 ? (
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
              handleOpenCreateRequest &&
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
