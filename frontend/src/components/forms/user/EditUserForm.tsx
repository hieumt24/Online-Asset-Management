import { LoadingSpinner } from "@/components/LoadingSpinner";
import { GenericDialog } from "@/components/shared";
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
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { GENDERS, ROLES } from "@/constants";
import { useLoading } from "@/context/LoadingContext";
import { useAuth } from "@/hooks";
import { UserRes } from "@/models";
import {
  getUserByIdService,
  resetPasswordService,
  updateUserService,
} from "@/services";
import { updateUserSchema } from "@/validations";
import { zodResolver } from "@hookform/resolvers/zod";
import { format } from "date-fns";
import { ChangeEvent, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";

import { toast } from "sonner";
import { z } from "zod";

export const EditUserForm = () => {
  const { userId } = useParams();
  const { isLoading, setIsLoading } = useLoading();
  const [userDetails, setUserDetails] = useState<UserRes>();
  const { user } = useAuth();
  const form = useForm<z.infer<typeof updateUserSchema>>({
    mode: "onBlur",
    resolver: zodResolver(updateUserSchema),
    defaultValues: {
      dateOfBirth: "",
      joinedDate: "",
      gender: "2",
      role: "2",
      location: user.location.toString(),
    },
  });

  const handleReset = async () => {
    setIsLoading(true);
    const result = await resetPasswordService(userDetails?.id!);
    if (result.success) {
      toast.success(result.message);
    } else {
      toast.error(result.message);
    }
    setIsLoading(false);
  };

  useEffect(() => {
    const fetchUser = async () => {
      setIsLoading(true);
      const res = await getUserByIdService(userId!);
      if (res.success) {
        const details = res.data;
        form.reset({
          dateOfBirth: format(details.dateOfBirth, "yyyy-MM-dd"),
          joinedDate: format(details.joinedDate, "yyyy-MM-dd"),
          gender: details.gender.toString(),
          role: details.role.toString(),
          location: details.location.toString(),
        });
        setUserDetails(details);
      } else {
        toast.error(res.message);
      }
      setIsLoading(false);
    };
    fetchUser();
  }, [userId]);

  const onSubmit = async (values: z.infer<typeof updateUserSchema>) => {
    const gender = parseInt(values.gender);
    const role = parseInt(values.role);
    try {
      setIsLoading(true);
      const res = await updateUserService({
        ...values,
        gender,
        role,
        userId: userDetails?.id ? userDetails?.id : "",
      });
      if (res.success) {
        toast.success(res.message);
        localStorage.setItem("edited", "1");
      } else {
        toast.error(res.message);
      }
    } catch (err) {
      console.log(err);
      toast.error("Error updating user");
    } finally {
      setIsLoading(false);
      navigate("/users");
    }
  };

  const navigate = useNavigate();

  if (isLoading) return <LoadingSpinner />;

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="w-1/3 space-y-5 rounded-2xl bg-white p-6 shadow-md"
      >
        <div className="flex justify-between">
          <h1 className="text-2xl font-bold text-red-600">Edit User</h1>
          <div>
            <GenericDialog
              trigger="Reset password"
              title="Reset password"
              desc="Are you sure to reset password of this account?"
              confirmText="Reset"
              cancelText="Cancel"
              onConfirm={handleReset}
            />
          </div>
        </div>
        <FormField
          name="staffCode"
          render={() => (
            <FormItem>
              <FormLabel className="text-md">Staff code</FormLabel>
              <FormControl>
                <Input value={userDetails?.staffCode} disabled />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          name="username"
          render={() => (
            <FormItem>
              <FormLabel className="text-md">Username</FormLabel>
              <FormControl>
                <Input value={userDetails?.username} disabled />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          name="firstName"
          render={() => (
            <FormItem>
              <FormLabel className="text-md">First Name</FormLabel>
              <FormControl>
                <Input value={userDetails?.firstName} disabled />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          name="lastName"
          render={() => (
            <FormItem>
              <FormLabel className="text-md">Last Name</FormLabel>
              <FormControl>
                <Input value={userDetails?.lastName} disabled />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="dateOfBirth"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Date of birth <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Input
                  {...field}
                  type="date"
                  className="justify-center"
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    const value = e.target.value;
                    const parts = value.split("-");
                    if (parts.length > 0 && parts[0].length > 4) {
                      // If the year part is longer than 4 digits, truncate it
                      const truncatedValue = `${parts[0].substring(0, 4)}-${parts[1]}-${parts[2]}`;
                      field.onChange({
                        ...e,
                        target: {
                          ...e.target,
                          value: truncatedValue,
                        },
                      });
                    } else {
                      field.onChange(e);
                    }
                  }}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="joinedDate"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Joined Date <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Input
                  {...field}
                  type="date"
                  className="justify-center"
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    const value = e.target.value;
                    const parts = value.split("-");
                    if (parts.length > 0 && parts[0].length > 4) {
                      // If the year part is longer than 4 digits, truncate it
                      const truncatedValue = `${parts[0].substring(0, 4)}-${parts[1]}-${parts[2]}`;
                      field.onChange({
                        ...e,
                        target: {
                          ...e.target,
                          value: truncatedValue,
                        },
                      });
                    } else {
                      field.onChange(e);
                    }
                  }}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="gender"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">Gender</FormLabel>
              <FormControl>
                <RadioGroup
                  onValueChange={field.onChange}
                  {...field}
                  className="flex gap-8"
                >
                  {GENDERS.map((gender) => {
                    return (
                      <FormItem
                        className="flex items-center space-x-3 space-y-0"
                        key={gender.value}
                      >
                        <FormControl>
                          <RadioGroupItem value={gender.value.toString()} />
                        </FormControl>
                        <FormLabel className="font-normal">
                          {gender.label}
                        </FormLabel>
                      </FormItem>
                    );
                  })}
                </RadioGroup>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="role"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">Types</FormLabel>
              <FormControl>
                <Select
                  {...field}
                  onValueChange={field.onChange}
                  disabled={userDetails?.role === 1}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Role" />
                  </SelectTrigger>
                  <SelectContent>
                    {ROLES.map((role) => (
                      <SelectItem
                        key={role.value}
                        value={role.value.toString()}
                      >
                        {role.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <div className="flex justify-end gap-4">
          <Button
            type="submit"
            className="w-[76px] bg-red-500 hover:bg-white hover:text-red-500"
            disabled={!form.formState.isValid || isLoading}
          >
            {isLoading ? "Saving..." : "Save"}
          </Button>
          <Button
            type="button"
            className="w-[76px] border bg-white text-black shadow-none hover:text-white"
            onClick={() => {
              navigate("/users");
            }}
          >
            Cancel
          </Button>
        </div>
      </form>
    </Form>
  );
};
