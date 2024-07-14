import { HiDocumentReport } from "react-icons/hi";
import { LuComputer, LuHome, LuListChecks, LuUserCog } from "react-icons/lu";
import { MdAssignmentReturn } from "react-icons/md";

export const ADMIN_NAV_FUNCTIONS = [
  {
    icon: <LuHome />,
    name: "Home",
    path: "/home",
  },
  {
    icon: <LuUserCog />,
    name: "Manage User",
    path: "/users",
  },
  {
    icon: <LuComputer />,
    name: "Manage Asset",
    path: "/assets",
  },
  {
    icon: <LuListChecks />,
    name: "Manage Assignment",
    path: "/assignments",
  },
  {
    icon: <MdAssignmentReturn />,
    name: "Request for Returning",
    path: "/returning-request",
  },
  {
    icon: <HiDocumentReport />,
    name: "Report",
    path: "/reports",
  },
];

export const STAFF_NAV_FUNCTIONS = [
  {
    icon: <LuHome />,
    name: "Home",
    path: "/home",
  },
];

export const BREADCRUMB_COMPONENTS = [
  {
    name: "Create New User",
    path: "/users/create",
    link: "#",
  },
  {
    name: "Edit User",
    path: "/users/edit/",
    link: "#",
  },
  {
    name: "Create New Asset",
    path: "/assets/create",
    link: "#",
  },
  {
    name: "Edit Asset",
    path: "/assets/edit",
    link: "#",
  },
  {
    name: "Create Assignment",
    path: "/assignments/create",
    link: "#",
  },
  {
    name: "Edit Assignment",
    path: "/assignments/edit",
    link: "#",
  },
];

export const GENDERS = [
  {
    value: 2,
    label: "Male",
  },
  {
    value: 3,
    label: "Female",
  },
];

export const ROLES = [
  {
    value: 1,
    label: "Admin",
  },
  {
    value: 2,
    label: "Staff",
  },
];

export const LOCATIONS = [
  {
    value: 1,
    label: "Ha Noi",
  },
  {
    value: 2,
    label: "Da Nang",
  },
  {
    value: 3,
    label: "Ho Chi Minh",
  },
];

export const ASSET_STATES = [
  {
    value: 1,
    label: "Available",
  },
  {
    value: 2,
    label: "Not available",
  },
  {
    value: 3,
    label: "Assigned",
  },
  {
    value: 4,
    label: "Waiting for Recycling",
  },
  {
    value: 5,
    label: "Recycled",
  },
];

export const ASSIGNMENT_STATES = [
  {
    value: 1,
    label: "Accepted",
  },
  {
    value: 2,
    label: "Waiting for acceptance",
  },
  {
    value: 3,
    label: "Declined",
  },
  {
    value: 4,
    label: "Waiting for returning",
  },
  {
    value: 5,
    label: "Returned",
  },
];

export const RETURNING_REQUEST_STATES = [
  {
    value: 1,
    label: "Waiting for returning",
  },
  {
    value: 2,
    label: "Completed",
  },
];
