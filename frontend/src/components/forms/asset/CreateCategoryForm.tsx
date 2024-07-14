import { FullPageModal } from "@/components/FullPageModal";
import { Button } from "@/components/ui/button";
import { Dialog, DialogContent, DialogHeader } from "@/components/ui/dialog";
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
import { removeExtraWhitespace } from "@/lib/utils";
import { createCategoryService } from "@/services/admin/manageAssetService";
import { createCategorySchema } from "@/validations/categorySchema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";

interface DialogProps {
  open: boolean;
  setOpen: (open: boolean) => void;
  onCreate: () => void;
}

export const CreateCategoryForm = (props: DialogProps) => {
  const { open, setOpen, onCreate } = props;
  const form = useForm<z.infer<typeof createCategorySchema>>({
    mode: "onBlur",
    resolver: zodResolver(createCategorySchema),
    defaultValues: {
      categoryName: "",
      prefix: "",
    },
  });

  const { setIsLoading } = useLoading();

  const handleSubmit = async (values: z.infer<typeof createCategorySchema>) => {
    try {
      setIsLoading(true);
      const res = await createCategoryService({ ...values });
      if (res.success) {
        toast.success(res.message);
      } else {
        toast.error(res.message);
      }
      onCreate();
    } catch (error) {
      console.log(error);
      toast.error("Error creating category");
    } finally {
      form.reset();
      setIsLoading(false);
      setOpen(false);
    }
  };

  const { isLoading } = useLoading();

  return (
    <FullPageModal show={open}>
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent>
          <DialogHeader className="text-xl font-semibold text-red-600">
            Add new category
          </DialogHeader>
          <Form {...form}>
            <form action="" onSubmit={form.handleSubmit(handleSubmit)}>
              <div className="flex gap-2">
                <FormField
                  control={form.control}
                  name="categoryName"
                  render={({ field }) => (
                    <FormItem className="w-1/2">
                      <FormLabel className="text-md">
                        Name <span className="text-red-600">*</span>
                      </FormLabel>
                      <FormControl>
                        <Input
                          placeholder="Enter category name"
                          {...field}
                          onBlur={(e) => {
                            const cleanedValue = removeExtraWhitespace(
                              e.target.value,
                            );
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
                  name="prefix"
                  render={({ field }) => (
                    <FormItem className="w-1/2">
                      <FormLabel className="text-md">
                        Prefix <span className="text-red-600">*</span>
                      </FormLabel>
                      <FormControl>
                        <Input
                          placeholder="Enter category prefix"
                          {...field}
                          onBlur={(e) => {
                            const cleanedValue = removeExtraWhitespace(
                              e.target.value,
                            );
                            field.onChange(cleanedValue);
                            field.onBlur();
                          }}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <div className="mt-4 flex items-center justify-end gap-4">
                <Button
                  variant={"destructive"}
                  type="submit"
                  disabled={!form.formState.isValid || isLoading}
                >
                  {isLoading ? "Creating..." : "Create"}
                </Button>
                <Button
                  variant="outline"
                  onClick={() => {
                    setOpen(false);
                    form.reset();
                  }}
                >
                  Cancel
                </Button>
              </div>
            </form>
          </Form>
        </DialogContent>
      </Dialog>
    </FullPageModal>
  );
};
