import { LoadingSpinner } from "@/components/LoadingSpinner";
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
import { Textarea } from "@/components/ui/textarea";
import { useLoading } from "@/context/LoadingContext";
import { removeExtraWhitespace } from "@/lib/utils";
import { AssetRes } from "@/models";
import {
  getAssetByIdService,
  updateAssetService,
} from "@/services/admin/manageAssetService";
import { updateAssetSchema } from "@/validations/assetSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { format } from "date-fns";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";

import { toast } from "sonner";
import { z } from "zod";

export const EditAssetForm: React.FC = () => {
  const { assetId } = useParams();

  const { isLoading, setIsLoading } = useLoading();
  const navigate = useNavigate();
  const [asset, setAsset] = useState<AssetRes>();

  const form = useForm<z.infer<typeof updateAssetSchema>>({
    mode: "onBlur",
    resolver: zodResolver(updateAssetSchema),
    defaultValues: {
      name: "",
      specification: "",
      installedDate: "",
      state: "1",
    },
  });

  const fetchAsset = async () => {
    try {
      setIsLoading(true);
      const res = await getAssetByIdService(assetId ? assetId : "");
      const details = res.data;
      if (res.success) {
        setAsset(res.data);
        form.reset({
          name: details.assetName,
          specification: details.specification,
          installedDate: format(details.installedDate, "yyyy-MM-dd"),
          state: details.state.toString(),
        });
      } else {
        toast.error(res.message);
      }
      setIsLoading(false);
    } catch (error) {
      console.log(error);
      toast.error("Error fetching asset");
    }
  };

  useEffect(() => {
    if (assetId) {
      fetchAsset();
    }
  }, [assetId]);

  const onSubmit = async (values: z.infer<typeof updateAssetSchema>) => {
    try {
      setIsLoading(true);
      const res = await updateAssetService(asset?.id ? asset?.id : "", {
        assetName: values.name,
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

  if (isLoading) return <LoadingSpinner />;

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="w-1/3 space-y-5 rounded-2xl bg-white p-6 shadow-md"
      >
        <h1 className="text-2xl font-bold text-red-600">Edit Asset</h1>
        <FormField
          name="assetCode"
          render={() => (
            <FormItem>
              <FormLabel className="text-md">Asset code</FormLabel>
              <FormControl>
                <Input disabled value={asset?.assetCode} />
              </FormControl>
            </FormItem>
          )}
        />
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
                  autoFocus
                />
              </FormControl>
              <FormMessage>{form.formState.errors.name?.message}</FormMessage>
            </FormItem>
          )}
        />
        <FormField
          name="category"
          render={() => (
            <FormItem>
              <FormLabel className="text-md">Category</FormLabel>
              <FormControl>
                <Input disabled value={asset?.categoryName} />
              </FormControl>
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
                <Textarea placeholder="Enter specification" {...field} onBlur={(e) => {
                    field.onChange(e.target.value.trim());
                    field.onBlur();
                  }}/>
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
                  style={{ justifyContent: "center" }}
                  type="date"
                  {...field}
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
                  className=""
                  disabled={asset?.state === 3}
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
                  <FormItem className="flex items-center gap-1 space-y-0">
                    <FormControl>
                      <RadioGroupItem value={"4"} />
                    </FormControl>
                    <FormLabel className="font-normal">
                      Waiting for Recycling
                    </FormLabel>
                  </FormItem>
                  <FormItem className="flex items-center gap-1 space-y-0">
                    <FormControl>
                      <RadioGroupItem value={"5"} />
                    </FormControl>
                    <FormLabel className="font-normal">Recycled</FormLabel>
                  </FormItem>
                  {asset?.state === 3 && (
                    <FormItem className="flex items-center gap-1 space-y-0">
                      <FormControl>
                        <RadioGroupItem value={"3"} />
                      </FormControl>
                      <FormLabel className="font-normal">Assigned</FormLabel>
                    </FormItem>
                  )}
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
            onClick={() => navigate("/assets")}
          >
            Cancel
          </Button>
        </div>
      </form>
    </Form>
  );
};
