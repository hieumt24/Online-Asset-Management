import { isAfter, isBefore, isValid } from "date-fns";
import { z } from "zod";

const dateFormat = /^\d{4}-?\d{2}-?\d{2}$/;
const nameFormat = /^(?=.*[a-zA-ZÀ-ÿĀ-žḀ-ỿơư])[\p{L}\p{N}\p{P}\p{S}\s]+$/u;

export const createAssetSchema = z.object({
  name: z
    .string()
    .trim()
    .min(1, "The asset name must not be blank")
    .min(2, { message: "The asset name must be at least 2 characters long." })
    .max(50, {
      message: "The asset name must be no longer than 50 characters.",
    }) 
    .regex(nameFormat, "The asset name must contain letters."),
  category: z.string().min(1, "Category is required."),
  specification: z
    .string()
    .trim()
    .min(1, { message: "Specification must not be blank" })
    .min(2, { message: "Specification must be at least 2 characters long." })
    .max(100, {
      message: "Specification must be no longer than 100 characters.",
    })
  .regex(nameFormat, "Specification must contain letters."),
  installedDate: z
    .string()
    .regex(dateFormat, { message: "Please select a valid Installed Date." })
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return isValid(parsedDate);
      },
      { message: "Invalid Installed Date. Please enter a valid date." },
    )
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return !isBefore(parsedDate, new Date("2000/01/01"));
      },
      { message: "Installed Date must be from the year 2000 or later." },
    )
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return !isAfter(parsedDate, new Date());
      },
      { message: "Installed Date cannot be in the future." },
    ),
  state: z.enum(["1", "2"]),
});

export const updateAssetSchema = z.object({
  name: z
    .string()
    .trim()
    .min(1, "The asset name must not be blank")
    .min(2, { message: "The asset name must be at least 2 characters long." })
    .max(50, {
      message: "The asset name must be no longer than 50 characters.",
    })
    .regex(nameFormat, {
      message: "The asset name must contain letters.",
    }),
  specification: z
    .string()
    .trim()
    .min(1, { message: "Specification must not be blank" })
    .min(2, { message: "Specification must be at least 2 characters long." })
    .max(100, {
      message: "Specification must be no longer than 100 characters.",
    })
    .regex(nameFormat, "Specification must contain letters."),
  installedDate: z
    .string()
    .regex(dateFormat, { message: "Please select a valid Installed Date." })
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return isValid(parsedDate);
      },
      { message: "Invalid Installed Date. Please enter a valid date." },
    )
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return !isBefore(parsedDate, new Date("2000/01/01"));
      },
      { message: "Installed Date must be from the year 2000 or later." },
    )
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return !isAfter(parsedDate, new Date());
      },
      { message: "Installed Date cannot be in the future." },
    ),
  state: z.enum(["1", "2", "3", "4", "5"]),
});
