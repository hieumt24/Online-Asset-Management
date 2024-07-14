import { GenericDialog, SearchForm } from "@/components";
import { LoadingSpinner } from "@/components/LoadingSpinner";
import { assetColumns } from "@/components/tables/asset/assetColumns";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ASSET_STATES } from "@/constants";
import { useLoading } from "@/context/LoadingContext";
import { useAuth, usePagination } from "@/hooks";
import { useAssets } from "@/hooks/useAssets";
import useClickOutside from "@/hooks/useClickOutside";
import { CategoryRes } from "@/models";
import {
  checkAssetHasAssignmentService,
  deleteAssetByIdService,
  getAllCategoryService,
} from "@/services";
import { useEffect, useRef, useState } from "react";
import { FaFilter } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { AssetTable } from "../../../../components/tables/asset/AssetTable";

export const ManageAsset = () => {
  const navigate = useNavigate();
  const { user } = useAuth();
  const { onPaginationChange, pagination } = usePagination();
  const [search, setSearch] = useState("");
  const [orderBy, setOrderBy] = useState("");
  const [isDescending, setIsDescending] = useState(true);
  const [assetStateType, setAssetStateType] = useState<number[]>([1, 2, 3]);
  const [selectAllState, setSelectAllState] = useState<boolean>(false);
  const [selectedCategory, setSelectedCategory] = useState<string>("all");
  const { assets, loading, error, pageCount, totalRecords, fetchAssets } =
    useAssets(
      pagination,
      user.location,
      search.trim(),
      orderBy,
      isDescending,
      assetStateType,
      selectedCategory,
    );
  const [isStateListOpen, setIsStateListOpen] = useState(false);
  const [categories, setCategories] = useState(Array<CategoryRes>);
  const [filteredCategories, setFilteredCategories] = useState(
    Array<CategoryRes>,
  );
  const [categorySearch, setCategorySearch] = useState("");
  const inputRef = useRef<HTMLInputElement>(null);

  const handleSelectAllChange = () => {
    if (selectAllState) {
      setAssetStateType([]);
    } else {
      setAssetStateType(ASSET_STATES.map((state) => state.value));
    }
  };

  useEffect(() => {
    setSelectAllState(assetStateType.length === ASSET_STATES.length);
  }, [assetStateType]);

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
    inputRef?.current?.focus();
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

  const [openDisable, setOpenDisable] = useState(false);
  const [openCannotDisable, setOpenCannotDisable] = useState(false);

  const { setIsLoading } = useLoading();
  const [assetIdToDelete, setAssetIdToDelete] = useState<string>("");
  const handleOpenDisable = async (id: string) => {
    setIsLoading(true);
    setAssetIdToDelete(id);
    const result = await checkAssetHasAssignmentService(id);
    if (result.success) {
      setOpenDisable(true);
    } else {
      setOpenCannotDisable(true);
    }
    setIsLoading(false);
  };

  const handleDelete = async () => {
    try {
      setIsLoading(true);
      const res = await deleteAssetByIdService(assetIdToDelete);
      if (res.success) {
        toast.success(res.message);
      } else {
        toast.error(res.message);
      }
      fetchAssets();
      setOpenDisable(false);
    } catch (err) {
      console.log(err);
      toast.error("Error when disable user");
    } finally {
      setIsLoading(false);
    }
  };

  const handleValueChange = (value: any) => {
    setSelectedCategory(value);
    pagination.pageIndex = 1;
  };

  const stateListRef = useRef<HTMLDivElement>(null);

  useClickOutside(stateListRef, ()=>{setIsStateListOpen(false)});

  const handleCheckboxChange = (stateValue: number) => {
    setAssetStateType((prev) => {
      if (prev.includes(stateValue)) {
        return prev.filter((value) => value !== stateValue);
      } else {
        return [...prev, stateValue];
      }
    });
    pagination.pageIndex = 1;
  };

  return (
    <div className="m-16 flex flex-grow flex-col gap-8">
      <div className="flex justify-between">
        <p className="text-2xl font-bold text-red-600">Asset List</p>
        <Button
          variant={"destructive"}
          onClick={() => navigate("/assets/create")}
        >
          <span className="capitalize">Create new asset</span>
        </Button>
      </div>

      <div className="flex items-center justify-between">
        <div className="flex gap-2">
          <Collapsible
            open={isStateListOpen}
            onOpenChange={setIsStateListOpen}
            className="relative w-[100px]"
            ref={stateListRef}
          >
            <CollapsibleTrigger asChild>
              <Button
                variant="ghost"
                size="sm"
                className="flex h-9 w-full items-center justify-between whitespace-nowrap rounded-md border border-input bg-transparent px-3 py-2 text-sm font-normal shadow-sm ring-offset-background placeholder:text-muted-foreground focus:outline-none disabled:cursor-not-allowed disabled:opacity-50 [&>span]:line-clamp-1"
              >
                State
                <FaFilter className="h-4 w-4 opacity-50" />
              </Button>
            </CollapsibleTrigger>
            <CollapsibleContent className="absolute z-10 max-h-96 w-[200px] overflow-hidden rounded-md border bg-popover bg-white p-1 font-semibold text-popover-foreground shadow-md transition-all data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[side=bottom]:slide-in-from-top-2">
              <div className="flex items-center gap-2 rounded-md px-2 py-1.5 text-sm font-normal text-zinc-900 transition-all hover:bg-zinc-100">
                <Checkbox
                  value="select-all"
                  id={`state-checkbox-select-all`}
                  onCheckedChange={handleSelectAllChange}
                  checked={selectAllState}
                />
                <label htmlFor={`state-checkbox-select-all`}>Select All</label>
              </div>
              {ASSET_STATES.map((state) => (
                <div
                  key={state.value}
                  className="flex items-center gap-2 rounded-md px-2 py-1.5 text-sm font-normal text-zinc-900 transition-all hover:bg-zinc-100"
                >
                  <Checkbox
                    value={state.value.toString()}
                    id={`state-checkbox-${state.value}`}
                    onCheckedChange={() => {
                      handleCheckboxChange(state.value);
                    }}
                    checked={assetStateType.includes(state.value)}
                  />
                  <label htmlFor={`state-checkbox-${state.value}`}>
                    {state.label}
                  </label>
                </div>
              ))}
            </CollapsibleContent>
          </Collapsible>
          <div className="min-w-[120px]">
            <Select onValueChange={handleValueChange} onOpenChange={()=>{setCategorySearch("")}}>
              <SelectTrigger>
                <SelectValue placeholder="Category" />
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
                  <SelectItem key={0} value="all">
                    All categories
                  </SelectItem>
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
              </SelectContent>
            </Select>
          </div>
        </div>
        <div className="flex justify-between gap-6">
          <SearchForm
            setSearch={setSearch}
            onSubmit={() => {
              onPaginationChange((prev) => ({
                ...prev,
                pageIndex: 1,
              }));
            }}
            placeholder="Asset code, asset name"
            className="w-[300px]"
          />
        </div>
      </div>
      {loading ? (
        <LoadingSpinner />
      ) : error ? (
        <div>Error</div>
      ) : (
        <>
          <AssetTable
            columns={assetColumns({
              handleOpenDisable,
              setOrderBy,
              setIsDescending,
              isDescending,
              orderBy,
            })}
            data={assets!}
            pagination={pagination}
            onPaginationChange={onPaginationChange}
            pageCount={pageCount}
            totalRecords={totalRecords}
          />
          <GenericDialog
            open={openDisable}
            setOpen={setOpenDisable}
            title="Are you sure"
            desc="Do you want to delete this asset"
            confirmText="Yes"
            cancelText="Cancel"
            onConfirm={handleDelete}
          />
          <GenericDialog
            title="Cannot delete asset"
            desc={`Cannot delete the asset because it belongs to one or more historical assignments. If the asset is not able to be used anymore, please update its state in <a href='/assets/edit/${assetIdToDelete}' class='text-blue-500 underline'>Edit Asset page.</a>`}
            open={openCannotDisable}
            setOpen={setOpenCannotDisable}
          />
        </>
      )}
    </div>
  );
};
