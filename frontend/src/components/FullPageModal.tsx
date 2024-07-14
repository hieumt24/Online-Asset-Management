// components/FullPageModal.tsx
import React from "react";

interface FullPageModalProps {
  show: boolean;
  children: React.ReactNode;
}

export const FullPageModal: React.FC<FullPageModalProps> = ({
  show,
  children,
}) => {
  if (!show) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div className="relative w-full max-w-md rounded-lg bg-white shadow-2xl">
        {children}
      </div>
    </div>
  );
};
