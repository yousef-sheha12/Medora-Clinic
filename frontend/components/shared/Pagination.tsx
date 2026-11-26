"use client"; // فيه handlers (onClick)، فلازم يشتغل في المتصفح

import { ChevronLeft, ChevronRight } from "lucide-react"; // أسهم السابق والتالي
import { Button } from "@/components/ui/button";          // زرار shadcn

interface PaginationProps {
  page: number;                         // الصفحة الحالية (بتبدأ من 1)
  totalCount: number;                   // العدد الكلي للعناصر (الـ count من الـ backend)
  pageSize: number;                     // عدد العناصر في الصفحة الواحدة
  onPageChange: (page: number) => void; // بتتنادى لما المستخدم يغير الصفحة
}

export default function Pagination({ page, totalCount, pageSize, onPageChange }: PaginationProps) {
  const totalPages = Math.max(1, Math.ceil(totalCount / pageSize)); // عدد الصفحات (على الأقل 1)

  if (totalPages <= 1) return null; // لو صفحة واحدة مفيش داعي للأزرار

  return (
    <nav aria-label="Pagination" className="mt-4 flex items-center justify-center gap-3">
      <Button
        variant="outline"
        size="sm"
        onClick={() => onPageChange(page - 1)}   // الصفحة اللي فاتت
        disabled={page <= 1}                     // مقفول في أول صفحة
      >
        <ChevronLeft className="size-4" aria-hidden="true" />
        Previous
      </Button>

      <span className="text-sm text-muted-foreground">
        Page {page} of {totalPages}              {/* مثلاً: Page 2 of 4 */}
      </span>

      <Button
        variant="outline"
        size="sm"
        onClick={() => onPageChange(page + 1)}   // الصفحة الجاية
        disabled={page >= totalPages}            // مقفول في آخر صفحة
      >
        Next
        <ChevronRight className="size-4" aria-hidden="true" />
      </Button>
    </nav>
  );
}