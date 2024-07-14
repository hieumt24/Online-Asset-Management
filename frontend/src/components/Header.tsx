import { useAuth } from "@/hooks";
import { useState } from "react";
import { LuKeyRound, LuLogOut, LuUser } from "react-icons/lu";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";

import { ChangePasswordForm } from "./forms/user/ChangePasswordForm";
import { MyBreadcrumb } from "./MyBreadcrumb";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "./ui/dropdown-menu";

import { GenericDialog } from "./shared";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import { Button } from "./ui/button";
import { Separator } from "./ui/separator";

export const Header = () => {
  const navigate = useNavigate();
  const { user, setIsAuthenticated } = useAuth();
  const [openLogout, setOpenLogout] = useState(false);
  const [openChangePassword, setOpenChangePassword] = useState(false);
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.setItem("logout", Date.now().toString());
    setIsAuthenticated(false);
    navigate("/auth/login");
    toast.success("You have been logged out");
  };

  const handleChangePassword = () => {
    setOpenChangePassword(true);
    setDropdownOpen(false);
  };

  const handleLogoutClick = () => {
    setOpenLogout(true);
    setDropdownOpen(false);
  };

  return (
    <header className="sticky top-0 z-50 w-full bg-red-600 p-4 shadow-md md:p-6">
      <div className="mx-auto flex items-center justify-between">
        <MyBreadcrumb />
        <DropdownMenu open={dropdownOpen} onOpenChange={setDropdownOpen}>
          <DropdownMenuTrigger asChild>
            <Button
              variant="ghost"
              className="flex items-center space-x-2 px-1 py-5 text-white hover:bg-red-700 hover:text-white"
            >
              <Avatar className="h-8 w-8 md:h-10 md:w-10">
                <AvatarImage src="/default_avatar.png" alt={user?.username} />
                <AvatarFallback>
                  {user?.username?.charAt(0).toUpperCase()}
                </AvatarFallback>
              </Avatar>
              <span className="hidden text-sm font-medium md:inline-block">
                {user?.username}
              </span>
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end" className="w-48 space-y-1">
            <DropdownMenuItem className="md:hidden">
              <LuUser className="mr-2 h-4 w-4" />
              <span className="text-sm font-medium">{user?.username}</span>
            </DropdownMenuItem>
            <Separator className="md:hidden" />
            <DropdownMenuItem onClick={handleChangePassword}>
              <LuKeyRound className="mr-2 h-4 w-4" />
              <span>Change Password</span>
            </DropdownMenuItem>
            <DropdownMenuItem onClick={handleLogoutClick}>
              <LuLogOut className="mr-2 h-4 w-4" />
              <span>Logout</span>
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>

        <ChangePasswordForm
          open={openChangePassword}
          onOpenChange={setOpenChangePassword}
        />

        <GenericDialog
          title="Are you sure?"
          desc="Do you want to logout?"
          confirmText="Log out"
          cancelText="Cancel"
          onConfirm={handleLogout}
          open={openLogout}
          setOpen={setOpenLogout}
        />
      </div>
    </header>
  );
};