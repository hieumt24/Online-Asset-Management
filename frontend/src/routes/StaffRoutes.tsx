import { MyAssignment } from "@/pages/MyAssignments";
import { Navigate, useRoutes } from "react-router-dom";

export const StaffRoutes = () => {
  const elements = useRoutes([
    {
      path: "/",
      element: <Navigate to="/home" />,
    },
    {
      path: "/home",
      element: <MyAssignment />,
    },
    {
      path: "/users",
      element: <Navigate to="/forbidden" />,
    },
    {
      path: "/users/create",
      element: <Navigate to="/forbidden" />,
    },
    {
      path: "/users/edit/:id",
      element: <Navigate to="/forbidden" />,
    },
    {
      path: "/assets",
      element: <Navigate to="/forbidden" />,
    },
    {
      path: "/assets/create",
      element: <Navigate to="/forbidden" />,
    },
    {
      path: "/assets/edit/:id",
      element: <Navigate to="/forbidden" />,
    },
    {
      path: "*",
      element: <Navigate to="/notfound" />,
    },
  ]);
  return elements;
};
