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
import { removeExtraWhitespace } from "@/lib/utils";
import { createUserService } from "@/services";
import { createUserSchema } from "@/validations";
import { zodResolver } from "@hookform/resolvers/zod";
import { CaretSortIcon } from "@radix-ui/react-icons";
import { ChangeEvent } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

import { toast } from "sonner";
import { z } from "zod";

export const CreateUserForm = () => {
  const { isLoading, setIsLoading } = useLoading();
  const { user } = useAuth();
  const form = useForm<z.infer<typeof createUserSchema>>({
    mode: "onBlur",
    resolver: zodResolver(createUserSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      dateOfBirth: "",
      joinedDate: "",
      gender: "2",
      role: "2",
      location: user.location.toString(),
    },
  });

  const onSubmit = async (values: z.infer<typeof createUserSchema>) => {
    const gender = parseInt(values.gender);
    const role = parseInt(values.role);
    const location = parseInt(values.location);
    setIsLoading(true);
    const res = await createUserService({
      ...values,
      gender,
      role,
      location,
    });

    setIsLoading(false);
    if (res.success) {
      toast.success(res.message);
      localStorage.setItem("edited", "1");
      navigate("/users");
    } else {
      toast.error(res.message);
    }
  };

  const navigate = useNavigate();

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="w-1/3 space-y-5 rounded-2xl bg-white p-6 shadow-md"
      >
        <h1 className="text-2xl font-bold text-red-600">Create New User</h1>
        <FormField
          control={form.control}
          name="firstName"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                First Name <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Input
                  placeholder="Enter first name"
                  {...field}
                  onBlur={(e) => {
                    const cleanedValue = removeExtraWhitespace(e.target.value); // Clean the input value
                    field.onChange(cleanedValue); // Update the form state
                    field.onBlur();
                  }}
                  autoFocus
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="lastName"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Last Name <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Input
                  placeholder="Enter last name"
                  {...field}
                  onBlur={(e) => {
                    const cleanedValue = removeExtraWhitespace(e.target.value);
                    field.onChange(cleanedValue);
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
                  defaultValue={field.value}
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
                <Select value={field.value} onValueChange={field.onChange}>
                  <SelectTrigger
                    icon={<CaretSortIcon className="ml-3 h-4 w-4 opacity-50" />}
                  >
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
        <FormField
          control={form.control}
          name="location"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input {...field} type="hidden" />
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
