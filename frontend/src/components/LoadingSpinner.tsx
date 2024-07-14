import { cn } from "@/lib/utils";
import React from "react";

interface LoadingSpinnerProps {
  className?: string;
}

export const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({
  className,
}) => {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-white bg-opacity-30">
      <div className={cn("relative", className)}></div>
      <div className="h-28 w-28 animate-spin rounded-full border-b-4 border-t-4 border-red-500"></div>
    </div>
  );
};
