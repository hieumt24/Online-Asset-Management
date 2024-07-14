import { SearchForm } from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { AssetTable, UserTable } from "@/components/tables";
import { assignmentAssetColumns } from "@/components/tables/asset/assignmentAssetColumns";
import { assignmentUserColumns } from "@/components/tables/user/assignmentUserColumns";
import { Button } from "@/components/ui/button";
import { Dialog, DialogContent, DialogTrigger } from "@/components/ui/dialog";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { useLoading } from "@/context/LoadingContext";
import { useAuth, usePagination, useUsers } from "@/hooks";
import { useAssets } from "@/hooks/useAssets";
import { AssetRes, UserRes } from "@/models";
import { createAssignmentService } from "@/services/admin/manageAssignmentService";
import { createAssignmentSchema } from "@/validations/assignmentSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { format } from "date-fns";
import { ChangeEvent, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { IoIosSearch } from "react-icons/io";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { z } from "zod";

export const CreateAssignmentForm = () => {
  const form = useForm<z.infer<typeof createAssignmentSchema>>({
    mode: "onBlur",
    resolver: zodResolver(createAssignmentSchema),
    defaultValues: {
      userId: "",
      assetId: "",
      assignedDate: format(new Date(), "yyyy-MM-dd"),
      note: "",
    },
  });

  const { isLoading, setIsLoading } = useLoading();

  const [openChooseUser, setOpenChooseUser] = useState(false);
  const [openChooseAsset, setOpenChooseAsset] = useState(false);

  const [userSearchQuery, setUserSearchQuery] = useState("");
  const [assetSearchQuery, setAssetSearchQuery] = useState("");

  const {
    onPaginationChange: onUsersPaginationChange,
    pagination: usersPagination,
  } = usePagination();

  const {
    onPaginationChange: onAssetsPaginationChange,
    pagination: assetsPagination,
  } = usePagination();

  const { user } = useAuth();

  const [usersOrderBy, setUsersOrderBy] = useState("");
  const [usersIsDescending, setUsersIsDescending] = useState(true);
  const [selectedUser, setSelectedUser] = useState<UserRes>();

  const [assetsOrderBy, setAssetsOrderBy] = useState("");
  const [assetsIsDescending, setAssetsIsDescending] = useState(true);
  const [selectedAsset, setSelectedAsset] = useState<AssetRes>();
  const [assetStateType] = useState<number[]>([1]);
  const [selectedCategory] = useState<string>("all");

  usersPagination.pageSize = 5;
  const { users, loading, pageCount, totalRecords } = useUsers(
    usersPagination,
    user.location,
    userSearchQuery,
    0,
    usersOrderBy,
    usersIsDescending,
  );

  assetsPagination.pageSize = 5;
  const {
    assets,
    loading: assetsLoading,
    pageCount: assetsPageCount,
    totalRecords: assetsTotalRecords,
  } = useAssets(
    assetsPagination,
    user.location,
    assetSearchQuery,
    assetsOrderBy,
    assetsIsDescending,
    assetStateType,
    selectedCategory,
  );

  const navigate = useNavigate();

  const handleSubmitForm = async (
    values: z.infer<typeof createAssignmentSchema>,
  ) => {
    try {
      setIsLoading(true);
      const res = await createAssignmentService({
        ...values,
        assignedIdTo: values.userId,
        assignedIdBy: user.id,
        location: user.location,
        state: 2,
      });
      if (res.success) {
        toast.success(res.message);
        localStorage.setItem("edited", "1");
        navigate("/assignments");
      } else {
        toast.error(res.message);
      }
    } catch (error) {
      console.log(error);
      toast.error("Error assigning an asset to user");
    } finally {
      form.reset();
      setIsLoading(false);
    }
  };

  useEffect(() => {
    setUserSearchQuery("");
  }, [openChooseUser]);

  useEffect(() => {
    setAssetSearchQuery("");
  }, [openChooseAsset]);

  return (
    <Form {...form}>
      <form
        className="w-1/3 space-y-5 rounded-2xl bg-white p-6 shadow-md"
        onSubmit={form.handleSubmit(handleSubmitForm)}
      >
        <h1 className="text-2xl font-bold text-red-600">
          Create New Assignment
        </h1>
        <FormField
          control={form.control}
          name="userId"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                User <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Dialog
                  open={openChooseUser}
                  onOpenChange={async (open) => {
                    setOpenChooseUser(open);
                    if (!open) {
                      await form.trigger("userId");
                    }
                  }}
                >
                  <DialogTrigger className="flex min-h-10 w-full items-center justify-between rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors">
                    <span className="w-full text-left text-zinc-500 break-all">
                      {form.getValues("userId") !== ""
                        ? `${selectedUser?.staffCode} - ${selectedUser?.firstName} ${selectedUser?.lastName}`
                        : "Choose user"}
                    </span>
                    <Input type="hidden" {...field} />
                    <IoIosSearch />
                  </DialogTrigger>
                  <DialogContent className="max-w-2xl">
                    <div className="max-w-[622px] px-6">
                      <div className="flex w-full justify-between">
                        <div className="flex items-center text-lg font-bold text-red-600">
                          Select User
                        </div>
                        <SearchForm
                          setSearch={setUserSearchQuery}
                          onSubmit={() => {
                            onUsersPaginationChange((prev) => ({
                              ...prev,
                              pageIndex: 1,
                            }));
                          }}
                          placeholder="Staff code, username, full name"
                          className="w-[300px]"
                        />
                      </div>
                      <div className="mt-8">
                        {loading ? (
                          <div className="h-[400px]">
                            <LoadingSpinner />
                          </div>
                        ) : (
                          <UserTable
                            columns={assignmentUserColumns({
                              selectedId: selectedUser?.id || "",
                              setOrderBy: setUsersOrderBy,
                              setIsDescending: setUsersIsDescending,
                              isDescending: usersIsDescending,
                              orderBy: usersOrderBy,
                            })}
                            data={users!}
                            onPaginationChange={onUsersPaginationChange}
                            pagination={usersPagination}
                            pageCount={pageCount}
                            totalRecords={totalRecords}
                            onRowClick={setSelectedUser}
                            withIndex={false}
                            adjustablePageSize={false}
                          />
                        )}
                      </div>
                      <div className="mt-3 flex justify-end gap-2">
                        <Button
                          variant={"destructive"}
                          onClick={async () => {
                            form.setValue("userId", selectedUser?.id || "");
                            await form.trigger("userId");
                            setOpenChooseUser(false);
                          }}
                        >
                          Save
                        </Button>
                        <Button
                          variant={"ghost"}
                          onClick={async () => {
                            setOpenChooseUser(false);
                            await form.trigger("userId");
                          }}
                          className="border border-zinc-200 shadow-sm"
                        >
                          Cancel
                        </Button>
                      </div>
                    </div>
                  </DialogContent>
                </Dialog>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="assetId"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Asset <span className="text-red-600">*</span>
              </FormLabel>
              <FormControl>
                <Dialog
                  open={openChooseAsset}
                  onOpenChange={async (open) => {
                    setOpenChooseAsset(open);
                    if (!open) {
                      await form.trigger("assetId");
                    }
                  }}
                >
                  <DialogTrigger className="flex min-h-10 w-full items-center justify-between rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-sm transition-colors">
                    <span className="w-full text-left text-zinc-500 break-all">
                      {form.getValues("assetId") !== ""
                        ? `${selectedAsset?.assetCode} - ${selectedAsset?.assetName}`
                        : "Select asset"}
                    </span>
                    <Input type="hidden" {...field} />
                    <IoIosSearch />
                  </DialogTrigger>
                  <DialogContent className="max-w-2xl">
                    <div className="max-w-[622px] px-6">
                      <div className="flex w-full justify-between">
                        <div className="flex items-center text-lg font-bold text-red-600">
                          Select Asset
                        </div>
                        <SearchForm
                          setSearch={setAssetSearchQuery}
                          onSubmit={() => {
                            onAssetsPaginationChange((prev) => ({
                              ...prev,
                              pageIndex: 1,
                            }));
                          }}
                          placeholder="Asset code, asset name"
                          className="w-[300px]"
                        />
                      </div>
                      <div className="mt-8">
                        {assetsLoading ? (
                          <div className="h-[400px]">
                            <LoadingSpinner />
                          </div>
                        ) : (
                          <AssetTable
                            columns={assignmentAssetColumns({
                              selectedId: selectedAsset?.id || "",
                              setOrderBy: setAssetsOrderBy,
                              setIsDescending: setAssetsIsDescending,
                              isDescending: assetsIsDescending,
                              orderBy: assetsOrderBy,
                            })}
                            data={assets!}
                            onPaginationChange={onAssetsPaginationChange}
                            pagination={assetsPagination}
                            pageCount={assetsPageCount}
                            totalRecords={assetsTotalRecords}
                            onRowClick={setSelectedAsset}
                            withIndex={false}
                            adjustablePageSize={false}
                          />
                        )}
                      </div>
                      <div className="mt-3 flex justify-end gap-2">
                        <Button
                          variant={"destructive"}
                          onClick={async () => {
                            form.setValue("assetId", selectedAsset?.id || "");
                            await form.trigger("assetId");
                            setOpenChooseAsset(false);
                          }}
                        >
                          Save
                        </Button>
                        <Button
                          variant={"ghost"}
                          onClick={async () => {
                            setOpenChooseAsset(false);
                            await form.trigger("assetId");
                          }}
                          className="border border-zinc-200 shadow-sm"
                        >
                          Cancel
                        </Button>
                      </div>
                    </div>
                  </DialogContent>
                </Dialog>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="assignedDate"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">
                Assigned Date <span className="text-red-600">*</span>
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
                    console.log(value);
                  }}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="note"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="text-md">Note</FormLabel>
              <FormControl>
                <Textarea
                  placeholder="Enter note"
                  {...field}
                  onBlur={(e) => {
                    field.onChange(e.target.value.trim());
                    field.onBlur();
                  }}
                />
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
              navigate("/assignments");
            }}
          >
            Cancel
          </Button>
        </div>
      </form>
    </Form>
  );
};
