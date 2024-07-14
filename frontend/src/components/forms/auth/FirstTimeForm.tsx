import { FullPageModal } from "@/components/FullPageModal";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useLoading } from "@/context/LoadingContext";
import { useAuth } from "@/hooks";
import { firstTimeService } from "@/services";
import { firstTimeLoginSchema } from "@/validations";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

import { toast } from "sonner";
import { z } from "zod";

export const FirstTimeForm = () => {
  const { user, setIsAuthenticated } = useAuth();

  const isFirstTime = user.isFirstTimeLogin;
  const [showModal, setShowModal] = useState<boolean>(isFirstTime);
  const dateParts = user.dateOfBirth.split("-");
  const newDateStr = dateParts.join("");
  const oldPassword = user.username + "@" + newDateStr;

  // Define form
  const form = useForm<z.infer<typeof firstTimeLoginSchema>>({
    mode: "onBlur",
    resolver: zodResolver(firstTimeLoginSchema),
    defaultValues: {
      newPassword: "",
      currentPassword: oldPassword,
      username: user.username,
    },
  });

  useEffect(() => {
    if (isFirstTime) {
      setShowModal(true);
    }
    form.setValue("currentPassword", oldPassword);
    form.setValue("username", user.username);
  }, [user]);

  const navigate = useNavigate();
  const onSubmit = async (values: z.infer<typeof firstTimeLoginSchema>) => {
    const result = await firstTimeService({ ...values });
    if (result.success) {
      setIsAuthenticated(false);
      localStorage.removeItem("token");
      await toast.success(result.message);
      setShowModal(false);
      navigate("/auth/login");
    } else {
      toast.error(result.message);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setIsAuthenticated(false);
    navigate("/auth/login");
    toast.success("You have been logged out");
  };

  const { isLoading } = useLoading();

  return (
    <FullPageModal show={showModal}>
      <Form {...form}>
        <form
          onSubmit={form.handleSubmit(onSubmit)}
          className="w-full space-y-5 rounded-lg border-2 border-black bg-zinc-100 text-lg shadow-lg"
        >
          <h1 className="rounded-t-lg border-b-2 border-black bg-zinc-300 p-6 text-xl font-bold text-red-600">
            Change password
          </h1>
          {/* New password */}
          <div className="p-6">
            <div className="mb-6">
              <p>This is the first time you logged in.</p>
              <p>You have to change your password to continue.</p>
            </div>
            <FormField
              control={form.control}
              name="newPassword"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>New password</FormLabel>
                  <FormControl>
                    <Input
                      type="password"
                      placeholder="Enter your new password"
                      {...field}
                      onBlur={(e) => {
                        field.onChange(e.target.value);
                        field.onBlur();
                      }}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="currentPassword"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <Input type="hidden" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="username"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <Input type="hidden" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <div className="mt-6 flex justify-end gap-4">
              <Button
                type="submit"
                className="w-[76px] bg-red-500 hover:bg-white hover:text-red-500"
                disabled={!form.formState.isValid || isLoading}
              >
                {isLoading ? "Saving..." : "Save"}
              </Button>
              <Button
                type="submit"
                className="w-[76px] border bg-white text-black shadow-none hover:text-white"
                onClick={handleLogout}
              >
                Log out
              </Button>
            </div>
          </div>
        </form>
      </Form>
    </FullPageModal>
  );
};
