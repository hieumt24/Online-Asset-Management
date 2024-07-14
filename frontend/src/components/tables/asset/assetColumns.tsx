import { renderHeader } from "@/lib/utils";
import { AssetRes } from "@/models";
import { ColumnDef } from "@tanstack/react-table";
import { FiEdit2 } from "react-icons/fi";
import { IoCloseCircleOutline } from "react-icons/io5";
import { useNavigate } from "react-router-dom";

interface AssetColumnsProps {
  handleOpenDisable: (id: string) => void;
  setOrderBy: React.Dispatch<React.SetStateAction<string>>;
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>;
  isDescending: boolean;
  orderBy: string;
}

export const assetColumns = ({
  handleOpenDisable,
  setOrderBy,
  setIsDescending,
  isDescending,
  orderBy,
}: AssetColumnsProps): ColumnDef<AssetRes>[] => [
  {
    accessorKey: "assetCode",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "assetName",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
    cell: ({ row }) => {
      const asset = row.original;
      return <p className="max-w-[200px] overflow-hidden text-ellipsis" >{`${asset.assetName}`}</p>;
    },
  },
  {
    accessorKey: "categoryName",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
  },
  {
    accessorKey: "state",
    header: ({ column }) =>
      renderHeader(column, setOrderBy, setIsDescending, isDescending, orderBy),
    cell: ({ row }) => {
      const state = row.original.state;
      switch (state) {
        case 1:
          return <p className="text-green-600">Available</p>;
        case 2:
          return <p className="text-yellow-600">Not Available</p>;
        case 3:
          return <p className="text-red-600">Assigned</p>;
        case 4:
          return <p className="text-blue-600">Waiting For Recycling</p>;
        case 5:
          return <p className="text-gray-600">Recycled</p>;
        default:
          return <p>{}</p>;
      }
    },
  },
  {
    accessorKey: "action",
    header: "Actions",
    cell: ({ row }) => {
      const asset = row.original;
      // eslint-disable-next-line react-hooks/rules-of-hooks
      const navigate = useNavigate();
      return (
        <div className="flex gap-4">
          <button
            className="text-blue-500 hover:text-blue-700"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`edit/${asset.id}`);
            }}
            disabled={asset.state === 3}
            title="Edit Asset"
          >
            {asset.state == 3 ? (
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
              handleOpenDisable(asset.id!);
            }}
            disabled={asset.state === 3}
            title="Delete Asset"
          >
            {asset.state == 3 ? (
              <IoCloseCircleOutline
                size={20}
                className="text-black opacity-25 hover:text-black"
              />
            ) : (
              <IoCloseCircleOutline size={20} />
            )}
          </button>
        </div>
      );
    },
  },
  {
    accessorKey: "id",
    header: "",
    cell: ({ row }) => {
      const asset = row.original;
      return <div className="hidden">{asset.id}</div>;
    },
  },
];
