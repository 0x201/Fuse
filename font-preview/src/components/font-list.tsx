import { useState } from 'react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { cn } from '@/lib/utils';
import { ScrollArea } from "@/components/ui/scroll-area"

interface FontListProps {
  fonts: { family: string }[];
  selectedFont: string | null;
  onSelectFont: (family: string) => void;
  loading?: boolean;
}

export function FontList({ fonts, selectedFont, onSelectFont, loading }: FontListProps) {
  const [search, setSearch] = useState('');

  const filteredFonts = fonts.filter(font =>
    font.family.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="flex flex-col h-full border-r">
      <div className="p-4 border-b">
        <Input
          placeholder="搜索字体..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="w-full"
        />
        <div className="text-xs text-muted-foreground mt-2">
          共 {filteredFonts.length} 种字体
        </div>
      </div>
      <ScrollArea className="flex-1 min-h-0 rounded-md border">
        <div className="p-2 space-y-1">
          {loading ? (
            <div className="text-center py-8 text-muted-foreground">加载中...</div>
          ) : (
            filteredFonts.map((font) => (
              <Button
                key={font.family}
                variant="ghost"
                className={cn(
                  "w-full justify-start font-normal text-left",
                  selectedFont === font.family && "bg-accent text-accent-foreground"
                )}
                onClick={() => onSelectFont(font.family)}
                style={{ fontFamily: font.family }}
              >
                <span className="truncate">{font.family}</span>
              </Button>
            ))
          )}
          {!loading && filteredFonts.length === 0 && (
            <div className="text-center py-8 text-muted-foreground">没有找到匹配的字体</div>
          )}
        </div>
      </ScrollArea>
    </div>
  );
}