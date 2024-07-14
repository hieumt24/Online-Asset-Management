import { cn } from "@/lib/utils";
import * as React from "react";
import { LuEye, LuEyeOff } from "react-icons/lu";
import { useEffect, useState } from "react";

export interface InputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {}

const useFormattedDate = (inputId: string) => {
  const [formattedDate, setFormattedDate] = useState("");

  useEffect(() => {
    const handleDateChange = (e: Event) => {
      const target = e.target as HTMLInputElement;
      const date = new Date(target.value);
      if (!isNaN(date.getTime())) {
        const day = ("0" + date.getDate()).slice(-2);
        const month = ("0" + (date.getMonth() + 1)).slice(-2);
        const year = date.getFullYear();
        setFormattedDate(`${day}-${month}-${year}`);
      }
    };

    const dateInput = document.getElementById(inputId);
    if (dateInput) {
      dateInput.addEventListener("change", handleDateChange);
    }

    return () => {
      if (dateInput) {
        dateInput.removeEventListener("change", handleDateChange);
      }
    };
  }, [inputId]);

  return formattedDate;
};

const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, type, id, ...props }, ref) => {
    const [showPassword, setShowPassword] = React.useState<boolean>(false);
    const formattedDate = useFormattedDate(id || "");

    const handleTogglePassword = () => {
      setShowPassword(!showPassword);
    };

    return (
      <div className="relative flex flex-col">
        <input
          type={showPassword ? "text" : type}
          className={cn(
            `flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50 ${type === "password" ? "pr-8" : ""}`,
            className,
          )}
          value={formattedDate}
          id={id}
          ref={ref}
          {...props}
        />
        {type === "password" &&
          (showPassword ? (
            <LuEye
              className="absolute right-2 top-2"
              onClick={handleTogglePassword}
            />
          ) : (
            <LuEyeOff
              className="absolute right-2 top-2"
              onClick={handleTogglePassword}
            />
          ))}
      </div>
    );
  },
);

Input.displayName = "Input";

export { Input };
