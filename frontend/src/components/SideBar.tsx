import { ADMIN_NAV_FUNCTIONS, STAFF_NAV_FUNCTIONS } from "@/constants";
import { useAuth } from "@/hooks";
import React, { useEffect, useState } from "react";
import { LuChevronLeft, LuChevronRight } from "react-icons/lu";
import { useMediaQuery } from "react-responsive";
import { Link, useLocation } from "react-router-dom";
import { Button } from "./ui/button";
import { ScrollArea } from "./ui/scroll-area";

interface NavItem {
  icon: JSX.Element;
  path: string;
  name: string;
}

interface NavLinkProps {
  item: NavItem;
  isActive: boolean;
  collapsed: boolean;
  isDesktop: boolean;
}

const NavLink: React.FC<NavLinkProps> = ({
  item,
  isActive,
  collapsed,
  isDesktop,
}) => (
  <Link
    to={item.path}
    className={`flex w-full items-center gap-3 p-3 text-lg transition duration-200 ${collapsed ? "justify-center" : ""} ${
      isActive
        ? "bg-red-600 text-white"
        : "text-black hover:bg-red-600 hover:text-white"
    }`}
  >
    {collapsed && <span className="text-lg" title={item.name}>{item.icon}</span>}
    {!collapsed && (
      <span className={`${isDesktop ? "text-lg font-medium" : "text-sm"}`}>
        {item.name}
      </span>
    )}
  </Link>
);

export const Sidebar: React.FC = () => {
  const location = useLocation();
  const { user } = useAuth();
  const [collapsed, setCollapsed] = useState(false);
  const isDesktop = useMediaQuery({ query: "(min-width: 768px)" });

  useEffect(() => {
    setCollapsed(!isDesktop);
  }, [isDesktop]);

  const navItems: NavItem[] =
    user?.role === "Admin" ? ADMIN_NAV_FUNCTIONS : STAFF_NAV_FUNCTIONS;

  return (
    <aside
      className={`flex h-full flex-col bg-white shadow-md transition-all duration-300 ${
        collapsed ? "w-20" : isDesktop ? "w-72" : "w-60"
      }`}
    >
      <div
        className={`flex items-center justify-between p-4 ${collapsed ? "flex-col" : ""}`}
      >
        <Link
          to="/"
          className={`flex flex-col items-start ${collapsed ? "mb-4" : ""}`}
        >
          <img
            src="/logo.svg"
            alt="Logo"
            className="h-16 w-16 object-contain"
          />
          {!collapsed && (
            <span className="mt-2 text-xl font-bold text-red-600">
              Online Asset Management
            </span>
          )}
        </Link>
        <Button
          variant="ghost"
          size="icon"
          onClick={() => setCollapsed(!collapsed)}
          className={collapsed ? "mt-4" : ""}
        >
          {collapsed ? (
            <LuChevronRight className="h-6 w-6" />
          ) : (
            <LuChevronLeft className="h-6 w-6" />
          )}
        </Button>
      </div>

      <ScrollArea className="mx-2 bg-zinc-100">
        <nav className="my-4 px-2">
          {navItems.map((item) => (
            <NavLink
              key={item.path}
              item={item}
              isActive={location.pathname.includes(item.path)}
              collapsed={collapsed}
              isDesktop={isDesktop}
            />
          ))}
        </nav>
      </ScrollArea>
    </aside>
  );
};
