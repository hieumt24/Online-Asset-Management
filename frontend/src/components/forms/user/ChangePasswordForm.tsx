import { Button } from "@/components/ui/button";
import { Dialog, DialogContent } from "@/components/ui/dialog";
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
import { changePasswordSchema } from "@/validations";
import { zodResolver } from "@hookform/resolvers/zod";
import { Dispatch, SetStateAction } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

import { toast } from "sonner";
import { z } from "zod";

export const ChangePasswordForm = (props: {
  open: boolean;
  onOpenChange: Dispatch<SetStateAction<boolean>>;
}) => {
  const { open, onOpenChange } = props;
  const { user, setIsAuthenticated } = useAuth();
  const form = useForm<z.infer<typeof changePasswordSchema>>({
    mode: "onBlur",
    resolver: zodResolver(changePasswordSchema),
    defaultValues: {
      newPassword: "",
      currentPassword: "",
      username: user.username,
    },
  });

  const navigate = useNavigate();
  const onSubmit = async (values: z.infer<typeof changePasswordSchema>) => {
    const result = await firstTimeService({ ...values, username: user.username });
    if (result.success) {
      setIsAuthenticated(false);
      localStorage.removeItem("token");
      toast.success(result.message);
      onOpenChange(false);
      navigate("/auth/login");
    } else {
      toast.error(result.message);
    }
  };

  const { isLoading } = useLoading();

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="p-0 focus:outline-none">
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="w-full space-y-5 rounded-lg border-none bg-zinc-100 text-lg shadow-lg"
          >
            <h1 className="rounded-t-lg bg-zinc-300 p-6 text-xl font-bold text-red-600">
              Change password
            </h1>
            <div className="p-6">
              <FormField
                control={form.control}
                name="currentPassword"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Current password</FormLabel>
                    <FormControl>
                      <Input
                        type="password"
                        {...field}
                        placeholder="Enter your current password"
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
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
                      />
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
              </div>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
