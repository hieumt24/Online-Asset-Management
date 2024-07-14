import { isBefore, isValid, startOfDay } from "date-fns";
import { z } from "zod";

export const createAssignmentSchema = z.object({
  userId: z.string().min(1, "User is required."),
  assetId: z.string().min(1, "Asset is required."),
  assignedDate: z
    .string()
    .min(1, "Please select a valid date.")
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return isValid(parsedDate);
      },
      { message: "Please select a valid date." },
    )
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return !isBefore(parsedDate, startOfDay(new Date()));
      },
      { message: "Assigned Date can only be today or in the future." },
    ),
  note: z.preprocess(
    (val) => (val === "" ? undefined : val),
    z
      .string()
      .trim()
      .max(256, "Note must not be longer than 256 characters.")
      .optional(),
  ),
});

export const updateAssignmentSchema = z.object({
  userId: z.string().min(1, "User is required."),
  assetId: z.string().min(1, "Asset is required."),
  assignedDate: z
    .string()
    .min(1, "Please select a valid date.")
    .refine(
      (dateString) => {
        const parsedDate = new Date(dateString);
        return isValid(parsedDate);
      },
      { message: "Please select a valid date." },
    ),
  note: z.preprocess(
    (val) => (val === "" ? undefined : val),
    z
      .string()
      .trim()
      .max(256, "Note must not be longer than 256 characters.")
      .optional(),
  ),
});
