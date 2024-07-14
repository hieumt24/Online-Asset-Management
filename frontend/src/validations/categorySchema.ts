import { z } from "zod";

const containsLetters =
  /^(?=.*[a-zA-Z])[a-zA-Z0-9!@#$%^&*(),.?':{ }|<> ~`+=\-_ /\\[\] ]*$/;

export const createCategorySchema = z.object({
  categoryName: z
    .string()
    .min(1, { message: "Category name must not be blank" })
    .min(2, { message: "Category name must be at least 2 characters long." })
    .max(50, {
      message: "Category name must be no longer than 50 characters.",
    })
    .regex(containsLetters, { message: "Category name must contain letters." }),
  prefix: z
    .string()
    .min(1, { message: "Prefix must not be blank" })
    .min(2, { message: "Prefix must be at least 2 characters long." })
    .max(5, { message: "Prefix must be no longer than 5 characters." })
    .regex(/^[a-zA-Z\s]*$/, {
      message: "Prefix contains only letters.",
    })
    .regex(/^[A-Za-z]+$/, {
      message: "Prefix must only contain 1 word.",
    }),
});
