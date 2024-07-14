import { differenceInYears, isAfter, isValid } from "date-fns";
import { z } from "zod";

const dateFormat = /^\d{4}-?\d{2}-?\d{2}$/;
const nameFormat = /^[a-zA-Z\s]*$/;
const firstNameFormat = /^[A-Za-z]+$/;

export const createUserSchema = z
  .object({
    firstName: z
      .string()
      .trim()
      .min(1, "First Name must not be blank.")
      .min(2, { message: "First Name must be at least 2 letters long." })
      .max(50, { message: "First Name must be no longer than 50 letters." })
      .regex(nameFormat, {
        message: "First Name must only contain letters",
      })
      .regex(firstNameFormat, {
        message: "First Name must contain only 1 word.",
      }),
    lastName: z
      .string()
      .trim()
      .min(1, "Last Name must not be blank.")
      .min(2, { message: "Last Name must be at least 2 letters long." })
      .max(50, { message: "Last Name must be no longer than 50 letters." })
      .regex(nameFormat, {
        message: "Last Name must only contain letters and spaces.",
      }),
    dateOfBirth: z
      .string()
      .regex(dateFormat, {
        message: "Please select a valid Date Of Birth.",
      })
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return isValid(parsedDate);
        },
        { message: "Invalid Date of Birth. Please enter a valid date." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return !isAfter(parsedDate, new Date());
        },
        { message: "Date of Birth cannot be in the future." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          const age = differenceInYears(new Date(), parsedDate);
          return age >= 18;
        },
        { message: "Age must be at least 18 years old." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          const age = differenceInYears(new Date(), parsedDate);
          return age <= 65;
        },
        { message: "Age must be no more than 65 years old." },
      ),
    joinedDate: z
      .string()
      .regex(dateFormat, { message: "Please select a valid Joined Date." })
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return isValid(parsedDate);
        },
        { message: "Invalid Joined Date. Please enter a valid date." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return !isAfter(parsedDate, new Date());
        },
        { message: "Joined Date cannot be in the future." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);

          const dayOfWeek = parsedDate.toString().substring(0, 3);
          if (dayOfWeek === "Sat" || dayOfWeek === "Sun") {
            return false;
          }
          return true;
        },
        {
          message: "Joined date cannot be on Saturday, Sunday.",
        },
      ),
    gender: z.enum(["1", "2", "3"]),
    role: z.enum(["2", "1"]),
    location: z.string(),
  })
  .refine(
    (data) => {
      const dateOfBirth = new Date(data.dateOfBirth);
      const joinedDate = new Date(data.joinedDate);

      return isAfter(joinedDate, dateOfBirth);
    },
    {
      message: "Joined date must be after the date of birth.",
      path: ["joinedDate"],
    },
  );

export const searchSchema = z.object({
  searchTerm: z.string(),
});

export const updateUserSchema = z
  .object({
    dateOfBirth: z
      .string()
      .regex(dateFormat, {
        message: "Please select a valid Date Of Birth.",
      })
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return isValid(parsedDate);
        },
        { message: "Invalid Date of Birth. Please enter a valid date." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return !isAfter(parsedDate, new Date());
        },
        { message: "Date of Birth cannot be in the future." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          const age = differenceInYears(new Date(), parsedDate);
          return age >= 18;
        },
        { message: "Age must be at least 18 years old." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          const age = differenceInYears(new Date(), parsedDate);
          return age <= 65;
        },
        { message: "Age must be no more than 65 years old." },
      ),
    joinedDate: z
      .string()
      .regex(dateFormat, { message: "Please select a valid Joined Date." })
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return isValid(parsedDate);
        },
        { message: "Invalid Joined Date. Please enter a valid date." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);
          return !isAfter(parsedDate, new Date());
        },
        { message: "Joined Date cannot be in the future." },
      )
      .refine(
        (dateString) => {
          const parsedDate = new Date(dateString);

          const dayOfWeek = parsedDate.toString().substring(0, 3);
          if (dayOfWeek === "Sat" || dayOfWeek === "Sun") {
            return false;
          }
          return true;
        },
        {
          message: "Joined date cannot be on Saturday, Sunday.",
        },
      ),
    gender: z.enum(["1", "2", "3"]),
    role: z.enum(["2", "1"]),
    location: z.string(),
  })
  .refine(
    (data) => {
      const dateOfBirth = new Date(data.dateOfBirth);
      const joinedDate = new Date(data.joinedDate);

      return isAfter(joinedDate, dateOfBirth);
    },
    {
      message: "Joined date must be after the date of birth.",
      path: ["joinedDate"],
    },
  );

const specialCharacters = /^(?=.*[!@#$%^&*(),.?":{}|<>\-_+=;'\[\]`~]).*$/;

export const changePasswordSchema = z.object({
  currentPassword: z
    .string()
    .min(8, { message: "Password must be at least 8 characters long" })
    .max(50, { message: "Password must be less than 50 characters" }),
  newPassword: z
    .string()
    .min(8, { message: "Password must be at least 8 characters long" })
    .max(50, { message: "Password must be less than 50 characters" })
    .regex(/[A-Z]/, {
      message: "Password must contain at least one uppercase letter",
    })
    .regex(/[0-9]/, { message: "Password must contain at least one number" })
    .regex(specialCharacters, {
      message: "Password must contain at least one special character",
    }),
  username: z.string(),
});
