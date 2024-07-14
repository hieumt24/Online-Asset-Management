import { CreateAsset, CreateUser, ManageUser, Report } from "@/pages";
import { MyAssignment } from "@/pages/MyAssignments";
import { EditAsset } from "@/pages/admin/manage/asset/EditAsset";
import { ManageAsset } from "@/pages/admin/manage/asset/ManageAsset";
import { CreateAssignment } from "@/pages/admin/manage/assignment/CreateAssignment";
import { EditAssignment } from "@/pages/admin/manage/assignment/EditAssignment";
import { ManageAssignment } from "@/pages/admin/manage/assignment/ManageAssignment";
import { ManageReturningRequest } from "@/pages/admin/manage/returningRequest/ManageReturingRequest";
import { EditUser } from "@/pages/admin/manage/user/EditUser";
import { Navigate, useRoutes } from "react-router-dom";

export const AdminRoutes = () => {
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
      element: <ManageUser />,
    },
    {
      path: "/users/create",
      element: <CreateUser />,
    },
    {
      path: "/users/edit/:userId",
      element: <EditUser />,
    },
    {
      path: "/assets",
      element: <ManageAsset />,
    },
    {
      path: "/assets/create",
      element: <CreateAsset />,
    },
    {
      path: "/assets/edit/:assetId",
      element: <EditAsset />,
    },
    {
      path: "/assignments",
      element: <ManageAssignment />,
    },
    {
      path: "/assignments/create",
      element: <CreateAssignment />,
    },
    {
      path: "/assignments/edit/:assignmentId",
      element: <EditAssignment />,
    },
    {
      path: "/returning-request",
      element: <ManageReturningRequest />,
    },
    {
      path: "/reports",
      element: <Report />,
    },
    {
      path: "*",
      element: <Navigate to="/notfound" />,
    },
  ]);
  return elements;
};
