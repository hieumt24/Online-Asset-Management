import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import useDebounce from "@/hooks/useDebounce";
import { removeExtraWhitespace } from "@/lib/utils";
import { searchSchema } from "@/validations";
import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { MdClose, MdSearch } from "react-icons/md";

import { z } from "zod";

interface SearchFormProps {
  setSearch: React.Dispatch<React.SetStateAction<string>>;
  onSubmit?: any;
  placeholder?: string;
  className?: string;
}

export const SearchForm = (props: SearchFormProps) => {
  const form = useForm<z.infer<typeof searchSchema>>({
    mode: "all",
    resolver: zodResolver(searchSchema),
    defaultValues: {
      searchTerm: "",
    },
  });

  const [isInitialRender, setIsInitialRender] = useState(true);

  useEffect(() => {
    setIsInitialRender(false);
  }, []);

  useDebounce(
    () => {
      if (!isInitialRender) {
        props.setSearch(form.getValues("searchTerm"));
        if (props.onSubmit) {
          props.onSubmit();
        }
      }
    },
    [form.watch("searchTerm")],
    500,
  );

  const onSubmit = async (values: z.infer<typeof searchSchema>) => {
    props.setSearch(values.searchTerm);
    if (props.onSubmit) {
      props.onSubmit();
    }
  };

  const clearSearch = () => {
    form.reset({ searchTerm: "" });
    props.setSearch("");
    if (props.onSubmit) {
      props.onSubmit();
    }
  };

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className={`flex justify-between rounded-lg border border-zinc-200 text-lg ${props.className}`}
      >
        <FormField
          control={form.control}
          name="searchTerm"
          render={({ field }) => (
            <FormItem className="relative w-full">
              <FormControl>
                <Input
                  className="w-full border-none pr-16 focus-visible:ring-0"
                  placeholder={props.placeholder || "Search"}
                  {...field}
                  onBlur={(e) => {
                    const cleanedValue = removeExtraWhitespace(e.target.value);
                    field.onChange(cleanedValue);
                    field.onBlur();
                  }}
                />
              </FormControl>
              {field.value && (
                <Button
                  type="button"
                  variant="ghost"
                  onClick={clearSearch}
                  className="absolute -top-[22%] right-0 rounded-none px-3 py-1"
                >
                  <MdClose />
                </Button>
              )}
              <FormMessage />
            </FormItem>
          )}
        />
        <Button
          type="submit"
          variant="outline"
          className="rounded-s-none border-none p-2"
        >
          <MdSearch />
        </Button>
      </form>
    </Form>
  );
};
