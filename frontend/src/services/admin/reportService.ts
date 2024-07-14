import axiosInstance from "@/api/axiosInstance";
import { GetReportReq } from "@/models";
import { format } from "date-fns";
import { saveAs } from "file-saver";

export const getReportService = (req: GetReportReq) => {
  return axiosInstance
    .post("/reports", req)
    .then((res) => {
      return {
        success: true,
        message: "Report fetched successfully!",
        data: res.data,
      };
    })
    .catch((err) => {
      return {
        success: false,
        message: err.response?.data.message || "Failed to fetch report.",
        data: err,
      };
    });
};

export const exportReportService = async (location: number) => {
  try {

    const response = await axiosInstance.get(
      `/reports/export?location=${location}`,
      { responseType: "blob" },
    );
    const blob = new Blob([response.data], { type: response.data.type });
    saveAs(blob, `Report-${format(new Date(), "dd-MM-yyyy")}.xlsx`);
  } catch (error) {
    console.log(error);
  }
};
