import React from "react";
import { useNavigate } from "react-router-dom";

export const NotFound: React.FC = () => {
  const navigate = useNavigate();
  return (
    <div className="flex h-screen flex-col items-center justify-center bg-gradient-to-r from-white to-red-600 text-center">
      <div className="transform rounded-lg bg-white p-8 shadow-2xl transition-transform duration-300 hover:scale-105">
        <h1 className="text-6xl font-extrabold text-red-700">404</h1>
        <h2 className="mt-4 text-2xl font-semibold text-gray-800">
          Page Not Found
        </h2>
        <p className="mt-2 max-w-xs text-gray-600">
          The page you are looking for might have been removed or is temporarily
          unavailable.
        </p>
        <button
          className="mt-6 transform rounded-full bg-red-700 px-6 py-3 text-white transition-transform duration-300 hover:scale-105 hover:bg-red-600 focus:outline-none focus:ring-4 focus:ring-red-300"
          onClick={() => navigate("/")}
        >
          Back to Home
        </button>
      </div>
    </div>
  );
};
