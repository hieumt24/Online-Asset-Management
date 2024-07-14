import { LoginForm } from "@/components";
import { useAuth } from "@/hooks";
import { Navigate } from "react-router-dom";

export const Login: React.FC = () => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) return <Navigate to="/" />;

  return (
    <div className="flex h-screen flex-col items-center justify-center bg-gradient-to-br from-blue-100 to-red-100 dark:from-gray-800 dark:to-gray-900">
      <div className="mb-8 flex flex-col items-center justify-center gap-2 md:mb-24">
        <img src="/logo.svg" alt="Logo" className="h-20 w-20 sm:h-32 sm:w-32" />
        <p className="text-center text-xl font-bold text-red-600 sm:text-2xl">
          Rookies - Group 4
        </p>
      </div>
      <div className="mb-24 flex w-full justify-center border md:mb-48">
        <LoginForm />
      </div>
    </div>
  );
};
