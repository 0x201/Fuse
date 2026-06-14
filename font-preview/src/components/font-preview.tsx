import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from './ui/card';
import { Input } from './ui/input';
import { Label } from './ui/label';
import { Switch } from './ui/switch';
import { Slider } from './ui/slider';
import { Button } from './ui/button';

interface FontPreviewProps {
  fontFamily: string;
}

const LIGATURE_DEMO_TEXT = "=>  == === !=";

export function FontPreview({ fontFamily }: FontPreviewProps) {
  const [text, setText] = useState('The quick brown fox jumps over the lazy dog');
  const [fontSize, setFontSize] = useState<number | readonly number[]>([24]);
  const [ligaturesEnabled, setLigaturesEnabled] = useState(true);


  const insertDemoText = () => {
    setText(LIGATURE_DEMO_TEXT);
  };


  return (
    <Card className="w-full h-full">
      <CardHeader>
        <CardTitle>字体预览 - {fontFamily}</CardTitle>
        <div className="flex flex-col gap-4 mt-2">
          <div className="flex gap-2 items-end">
            <div className="flex-1">
              <Label htmlFor="preview-text">预览文本</Label>
              <Input
                id="preview-text"
                value={text}
                onChange={(e) => setText(e.target.value)}
                placeholder="输入要预览的文字"
              />
            </div>
            <Button
              type="button"
              variant="outline"
              onClick={insertDemoText}
              className="shrink-0"
            >
              连体演示文本
            </Button>
          </div>

          <div className="flex gap-4 items-center">
            <div className="flex-1">
              <Label htmlFor="font-size">字体大小: {fontSize}px</Label>
              <Slider
                id="font-size"
                min={12}
                max={72}
                step={1}
                value={fontSize}
                onValueChange={(val) => setFontSize(val)}
                className="mt-2"
              />
            </div>
            <div className="flex items-center gap-2">
              <Label htmlFor="ligature-switch" className="cursor-pointer">
                连体效果
              </Label>
              <Switch
                id="ligature-switch"
                checked={ligaturesEnabled}
                onCheckedChange={setLigaturesEnabled}
              />
            </div>
          </div>
        </div>
      </CardHeader>
      <CardContent>
        <div
          className="p-6 border rounded-lg bg-muted/20 transition-all"
          style={{
            fontFamily: fontFamily,
            fontSize: `${fontSize}px`,
            lineHeight: 1.4,
            fontVariantLigatures: ligaturesEnabled ? 'common-ligatures' : 'none',
            fontFeatureSettings: ligaturesEnabled ? '"liga" 1' : '"liga" 0',
          }}
        >
          {text || '预览文本'}
        </div>
        {!ligaturesEnabled && (
          <p className="text-xs text-muted-foreground mt-2">
            💡 连体效果已关闭，将不会合并为连字。
          </p>
        )}
      </CardContent>
    </Card>
  );
}