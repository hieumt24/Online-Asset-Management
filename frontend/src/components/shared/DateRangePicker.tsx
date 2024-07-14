import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import { format } from "date-fns";
import { useState } from "react";
import { DateRange } from "react-day-picker";
import { IoCalendar, IoClose } from "react-icons/io5";

interface DateRangePickerProps {
  formatDate?: string;
  setValue: any;
  placeholder?: string;
  onChange?: any;
  className?: string;
}

export function DateRangePicker(props: Readonly<DateRangePickerProps>) {
  const { setValue, placeholder, onChange, className } = props;
  const [dateRange, setDateRange] = useState<DateRange | undefined>();

  const handleDateRangeSelect = (selectedDate: DateRange | undefined) => {
    setDateRange(selectedDate);

    if (selectedDate) {
      setValue(selectedDate);
    }
    onChange && onChange();
  };

  const handleClear = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.stopPropagation(); // Prevent the Popover from opening
    setDateRange(undefined);
    setValue(null);
    onChange();
  };

  return (
    <Popover>
      <PopoverTrigger asChild>
        <Button
          variant={"outline"}
          className={cn(
            "min-w-48 items-center justify-between font-normal",
            !dateRange && "text-muted-foreground",
            className,
          )}
        >
          {dateRange?.from ? (
            dateRange.to ? (
              <>
                {format(dateRange.from, "dd/MM/yyyy")} -{" "}
                {format(dateRange.to, "dd/MM/yyyy")}
                <Button
                  variant="ghost"
                  size="icon"
                  className="h-4 w-4 p-0 mx-1"
                  onClick={handleClear}
                >
                  <IoClose size={14} />
                </Button>
              </>
            ) : (
              format(dateRange.from, "dd/MM/yyyy")
            )
          ) : (
            <span>{placeholder || "Select date"}</span>
          )}
          <IoCalendar size={20} />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-auto p-0">
        <Calendar
          mode={"range"}
          selected={dateRange}
          onSelect={handleDateRangeSelect}
          initialFocus
          captionLayout="dropdown-buttons"
          fromYear={2000}
          toYear={2099}
        />
      </PopoverContent>
    </Popover>
  );
}
