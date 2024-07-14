import { FirstTimeForm } from "@/components";
import { Header } from "@/components/Header";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { Sidebar } from "@/components/SideBar";
import { useAuth } from "@/hooks";
import { AdminRoutes } from "@/routes/AdminRoutes";
import { StaffRoutes } from "@/routes/StaffRoutes";

export const Layout = () => {
  const { user, loading } = useAuth();
  if (loading)
    return (
      <div className="flex h-full items-center">
        <LoadingSpinner />
      </div>
    );
  return (
    <div className="flex h-full flex-col items-start w-full">
      <FirstTimeForm />
      <Header />
      <div className="flex flex-grow w-full">
        <Sidebar />
        {user.role === "Admin" ? <AdminRoutes /> : <StaffRoutes />}
      </div>
    </div>
  );
};
