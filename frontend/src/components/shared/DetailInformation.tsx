import { ASSET_STATES, ASSIGNMENT_STATES, LOCATIONS } from "@/constants";
import { cn } from "@/lib/utils";
import { AssetRes, UserRes } from "@/models";
import { format } from "date-fns";
import { DialogContent } from "../ui/dialog";

interface DetailInformationProps<T> {
  info: T;
  variant: "User" | "Asset" | "Assignment";
}

type FormattableValue = string | number | Date | null | undefined;

export const DetailInformation = <T extends UserRes | AssetRes>({
  info,
  variant,
}: DetailInformationProps<T>) => {
  const formatValue = (key: string, value: FormattableValue): string => {
    if (value == null) return "N/A";

    const formatters: Record<string, (val: FormattableValue) => string> = {
      dateOfBirth: formatDate,
      joinedDate: formatDate,
      installedDate: formatDate,
      assignedDate: formatDate,
      gender: formatGender,
      role: formatRole,
      location: formatLocation,
      assetLocation: formatLocation,
      state: formatState,
    };

    return formatters[key]?.(value) ?? String(value);
  };

  const formatDate = (value: FormattableValue) =>
    (value instanceof Date) || (typeof value === "string") ? format(value, "dd/MM/yyyy") : String(value);

  const formatGender = (value: FormattableValue) =>
    value === 2 ? "Male" : value === 1 ? "Female" : "Other";

  const formatRole = (value: FormattableValue) =>
    value === 1 ? "Admin" : value === 2 ? "Staff" : "Unknown";

  const formatLocation = (value: FormattableValue) =>
    LOCATIONS[Number(value) - 1]?.label ?? "Unknown";

  const formatState = (value: FormattableValue): string => {
    const states = variant === "Assignment" ? ASSIGNMENT_STATES : ASSET_STATES;
    return states.find((state) => state.value === value)?.label ?? "";
  };

  const excludedKeys = [
    "id",
    "assetId",
    "assignedToId",
    "assignedById",
    "createdOn",
    "lastModifiedOn",
    "lastModifiedBy",
    "createdBy",
    "categoryId",
    "isDeleted",
  ];

  const formatKey = (key: string) =>
    key.replace(/([A-Z])/g, " $1").replace(/^./, (str) => str.toUpperCase());

  return (
    <DialogContent className="max-w-lg border-none p-0">
      <div className="overflow-hidden rounded-lg bg-white shadow-lg">
        <h2 className="bg-gradient-to-r from-red-600 to-red-800 p-6 text-2xl font-bold text-white">
          {variant} Details
        </h2>
        <div className="px-6 py-4">
          {info ? (
            <table className="w-full">
              <tbody>
                {Object.entries(info)
                  .filter(([key]) => !excludedKeys.includes(key))
                  .map(([key, value], index) => (
                    <tr
                      key={key}
                      className={cn(
                        "transition-colors hover:bg-gray-50",
                        index % 2 === 0 ? "bg-white" : "bg-gray-100",
                      )}
                    >
                      <td className="w-[40%] py-3 pr-4 font-semibold text-gray-700">
                        {formatKey(key)}
                      </td>
                      <td className="py-2 text-gray-800 overflow-auto whitespace-pre-wrap max-h-[120px] block">

                        {formatValue(key, value)}
                      </td>
                    </tr>
                  ))}
              </tbody>
            </table>
          ) : (
            <div className="text-center text-gray-500">
              No information available
            </div>
          )}
        </div>
      </div>
    </DialogContent>
  );
};
