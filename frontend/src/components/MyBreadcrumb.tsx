import { ADMIN_NAV_FUNCTIONS, BREADCRUMB_COMPONENTS } from "@/constants";
import React from "react";
import { Link, useLocation } from "react-router-dom";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbList,
  BreadcrumbSeparator,
} from "./ui/breadcrumb";

export const MyBreadcrumb = () => {
  const location = useLocation();

  return (
    <Breadcrumb className="flex">
      <BreadcrumbList className="text-xl font-bold text-white">
        {ADMIN_NAV_FUNCTIONS.map((item) => {
          return location.pathname.includes(item.path) ? (
            <BreadcrumbItem key={item.path}>
              <Link to={item.path}>{item.name}</Link>
            </BreadcrumbItem>
          ) : null;
        })}
        {BREADCRUMB_COMPONENTS.map((item) => {
          return location.pathname.includes(item.path) ? (
            <React.Fragment key={item.path}>
              <BreadcrumbSeparator />
              <BreadcrumbItem>
                <Link to={item.link}>{item.name}</Link>
              </BreadcrumbItem>
            </React.Fragment>
          ) : null;
        })}
      </BreadcrumbList>
    </Breadcrumb>
  );
};
