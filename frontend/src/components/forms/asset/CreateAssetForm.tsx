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
import { Textarea } from "@/components/ui/textarea";
import { useLoading } from "@/context/LoadingContext";
import { useAuth } from "@/hooks";
import { removeExtraWhitespace } from "@/lib/utils";
import { CategoryRes } from "@/models";
import {
  createAssetService,
  getAllCategoryService,
} from "@/services/admin/manageAssetService";
import { createAssetSchema } from "@/validations/assetSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { CaretSortIcon } from "@radix-ui/react-icons";
import { ChangeEvent, useEffect, useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

import { toast } from "sonner";
import { z } from "zod";
import { CreateCategoryForm } from "./CreateCategoryForm";

export const CreateAssetForm: React.FC = () => {
  const [categories, setCategories] = useState<Array<CategoryRes>>([]);
  const [filteredCategories, setFilteredCategories] = useState<
    Array<CategoryRes>
  >([]);
  const [openCreateCategory, setOpenCreateCategory] = useState(false);
  const [categorySearch, setCategorySearch] = useState("");
  const inputRef = useRef<HTMLInputElement>(null);
  const { user } = useAuth();
  const { isLoading, setIsLoading } = useLoading();
  const navigate = useNavigate();

  useEffect(() => {
    setFilteredCategories(
      categories.filter((category) =>
        category.categoryName
          .toLowerCase()
          .includes(categorySearch.toLowerCase()),
      ),
    );
  }, [categorySearch]);

  useEffect(() => {
    if (inputRef.current) {
      inputRef.current.focus();
    }
  }, [filteredCategories]);

  const fetchCategories = async () => {
    const res = await getAllCategoryService();
    if (res.success) {
      setCategories(res.data.data);
      setFilteredCategories(res.data.data);
    } else {
      console.log(res.message);
    }
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  const form = useForm<z.infer<typeof createAssetSchema>>({
    mode: "onBlur",
    resolver: zodResolver(createAssetSchema),
    defaultValues: {
      name: "",
      category: "",
      specification: "",
      installedDate: "",
      state: "1",
    },
  });

  const onSubmit = async (values: z.infer<typeof createAssetSchema>) => {
    const location = user.location;
    try {
      setIsLoading(true);
      const res = await createAssetService({
        adminId: user.id,
        assetName: values.name,
        assetLocation: location ? location : 1,
        categoryId: values.category,
        state: parseInt(values.state),
        specification: values.specification,
        installedDate: values.installedDate,
      });
      if (res.success) {
        toast.success("Asset created successfully!");
        localStorage.setItem("edited", "1");
        navigate("/assets");
      } else {
        toast.error(res.message);
      }
    } catch (error) {
      console.log(error);
      toast.error("Error creating asset");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="w-1/3 space-y-5 rounded-2xl bg-white p-6 shadow-md"
      >
        <h1 className="text-2xl font-bold text-red-600">Create New Asset</h1>

        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Name <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Input
                  placeholder="Enter asset name"
                  {...field}
                  onBlur={(e) => {
                    const cleanedValue = removeExtraWhitespace(e.target.value);
                    field.onChange(cleanedValue);
                    field.onBlur();
                  }}
                />
              </FormControl>
              <FormMessage>{form.formState.errors.name?.message}</FormMessage>
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="category"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Category <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Select {...field} onValueChange={field.onChange} onOpenChange={()=>{setCategorySearch("")}}>
                  <SelectTrigger
                    icon={<CaretSortIcon className="ml-3 h-4 w-4 opacity-50" />}
                  >
                    <SelectValue placeholder="Select category" />
                  </SelectTrigger>
                  <SelectContent>
                    <Input
                      ref={inputRef}
                      placeholder="Search category ..."
                      className="border-none shadow-none focus-visible:ring-0"
                      value={categorySearch}
                      onChange={(e) => {
                        setCategorySearch(e.target.value);
                      }}
                    />
                    <div className="max-h-[155px] overflow-y-scroll">
                      {categories.map((category) => (
                        <SelectItem
                          key={category.id}
                          value={category.id}
                          className={
                            filteredCategories.some((c) => c.id === category.id)
                              ? ""
                              : "hidden"
                          }
                        >
                          {category.categoryName} ({category.prefix})
                        </SelectItem>
                      ))}
                    </div>
                    <Button
                      variant={"ghost"}
                      className="w-full"
                      onClick={() => {
                        setOpenCreateCategory(true);
                      }}
                    >
                      + Add new category
                    </Button>
                  </SelectContent>
                </Select>
              </FormControl>
              <FormMessage>
                {form.formState.errors.category?.message}
              </FormMessage>
            </FormItem>
          )}
        />

        {/* Specification */}
        <FormField
          control={form.control}
          name="specification"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Specification <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Textarea
                  placeholder="Enter specification"
                  {...field}
                  onBlur={(e) => {
                    field.onChange(e.target.value.trim());
                    field.onBlur();
                  }}
                />
              </FormControl>
              <FormMessage>
                {form.formState.errors.specification?.message}
              </FormMessage>
            </FormItem>
          )}
        />

        {/* Installed Date */}
        <FormField
          control={form.control}
          name="installedDate"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Installed Date <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Input
                  id="formatted-date"
                  style={{ justifyContent: "center" }}
                  type="date"
                  {...field}
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
              <FormMessage>
                {form.formState.errors.installedDate?.message}
              </FormMessage>
            </FormItem>
          )}
        />

        {/* State */}
        <FormField
          control={form.control}
          name="state"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">State</FormLabel>
              <FormControl>
                <RadioGroup
                  onValueChange={field.onChange}
                  {...field}
                  className="flex gap-5"
                >
                  <FormItem className="flex items-center gap-1 space-y-0">
                    <FormControl>
                      <RadioGroupItem value={"1"} />
                    </FormControl>
                    <FormLabel className="font-normal">Available</FormLabel>
                  </FormItem>
                  <FormItem className="flex items-center gap-1 space-y-0">
                    <FormControl>
                      <RadioGroupItem value={"2"} />
                    </FormControl>
                    <FormLabel className="font-normal">Not available</FormLabel>
                  </FormItem>
                </RadioGroup>
              </FormControl>
              <FormMessage>{form.formState.errors.state?.message}</FormMessage>
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
              navigate("/assets");
            }}
          >
            Cancel
          </Button>
        </div>
        <CreateCategoryForm
          onCreate={fetchCategories}
          open={openCreateCategory}
          setOpen={setOpenCreateCategory}
        />
      </form>
    </Form>
  );
};
