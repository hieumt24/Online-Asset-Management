import { Button } from "@/components/ui/button";
import { Column } from "@tanstack/react-table";
import { clsx, type ClassValue } from "clsx";
import { IoMdArrowDropdown, IoMdArrowDropup } from "react-icons/io";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const removeExtraWhitespace = (str: string): string => {
  return str.replace(/\s+/g, " ").trim();
};

const handleHeaderClick = (
  columnId: string,
  setOrderBy: React.Dispatch<React.SetStateAction<string>>,
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>,
  orderBy: string,
) => {
  setOrderBy(columnId);
  setIsDescending((prev) => (orderBy === columnId ? !prev : true));
};

const formatHeader = (str: string): string => {
  return str
    .replace(/([a-z])([A-Z])/g, "$1 $2") // Add a space before each capital letter
    .replace(/^./, (char) => char.toUpperCase()); // Capitalize the first letter
};

export const renderHeader = <T,>(
  column: Column<T, unknown>,
  setOrderBy: React.Dispatch<React.SetStateAction<string>>,
  setIsDescending: React.Dispatch<React.SetStateAction<boolean>>,
  isDescending: boolean,
  orderBy: string,
  displayName?: string,
) => (
  <Button
    variant={"ghost"}
    onClick={() =>
      handleHeaderClick(column.id, setOrderBy, setIsDescending, orderBy)
    }
    className="p-0 hover:bg-muted/50"
  >
    <div className="flex items-center justify-center">
      <span
        className={`${orderBy === column.id ? "font-black text-red-600" : ""}`}
      >
        {displayName || formatHeader(column.id?.toString() || "")}
      </span>
      {isDescending && orderBy === column.id ? (
        <IoMdArrowDropup
          size={24}
          className={`${orderBy === column.id ? "font-black text-red-600" : ""}`}
        />
      ) : (
        <IoMdArrowDropdown
          size={24}
          className={`${orderBy === column.id ? "font-black text-red-600" : ""}`}
        />
      )}
    </div>
  </Button>
);
