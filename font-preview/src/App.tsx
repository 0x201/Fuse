import { useState } from 'react';
import { useSystemFonts } from './hooks/useSystemFonts';
import { FontList } from './components/font-list';
import { FontPreview } from './components/font-preview';
import { Alert, AlertDescription, AlertTitle } from './components/ui/alert';
import { Button } from './components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './components/ui/card';
import { ThemeProvider } from './components/theme-provider';
import { ModeToggle } from './components/mode-toggle';

function App() {
  const { fonts, loading, error, isApiSupported, isAuthorized, loadFonts, useFallbackFonts } = useSystemFonts();
  const [selectedFont, setSelectedFont] = useState<string | null>(null);
  const [hasAttemptedLoad, setHasAttemptedLoad] = useState(false);

  const handleLoadFonts = () => {
    loadFonts();
    setHasAttemptedLoad(true);
  };

  const handleUseFallback = () => {
    useFallbackFonts();
    setHasAttemptedLoad(true);
  };

  // 未尝试加载且 API 支持：显示授权按钮
  if (!hasAttemptedLoad && isApiSupported) {
    return (
      <div className="h-screen flex items-center justify-center p-4">
        <Card className="max-w-md w-full">
          <CardHeader>
            <CardTitle>字体预览器</CardTitle>
            <CardDescription>
              此应用需要读取您系统中的字体列表，以便选择和预览。
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <Button onClick={handleLoadFonts} className="w-full">
              授权并加载字体
            </Button>
            <Button variant="outline" onClick={handleUseFallback} className="w-full">
              使用预设字体列表（无需授权）
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  // 加载中状态
  if (loading) {
    return (
      <div className="h-screen flex items-center justify-center">
        <div className="text-muted-foreground">加载字体列表中...</div>
      </div>
    );
  }

  // 主界面
  return (
    <ThemeProvider>
      <div className="h-screen flex flex-col bg-muted/30">
        <header className="border-b bg-background/80 backdrop-blur-sm sticky top-0 z-10">
          <div className="flex justify-between items-center">
            <div>
              <h1 className="text-2xl font-bold">系统字体预览器</h1>
              <p className="text-muted-foreground text-sm">
                {isApiSupported
                  ? isAuthorized
                    ? '已授权，显示您系统中的真实字体'
                    : '正在使用预设字体列表'
                  : '浏览器不支持直接读取系统字体，使用预设列表'}
              </p>
            </div>
            {!isAuthorized && isApiSupported && (
              <Button variant="outline" onClick={handleLoadFonts}>
                重新授权获取字体
              </Button>
            )}
          </div>
          <ModeToggle />
        </header>

        <div className="flex-1 flex overflow-hidden">
          <div className="w-80 flex flex-col h-full">
            <FontList
              fonts={fonts}
              selectedFont={selectedFont}
              onSelectFont={setSelectedFont}
              loading={loading}
            />
          </div>
          <div className="flex-1 p-6 overflow-auto">
            {selectedFont ? (
              <FontPreview fontFamily={selectedFont} />
            ) : (
              <div className="flex items-center justify-center h-full text-muted-foreground">
                点击左侧字体开始预览
              </div>
            )}
          </div>
        </div>

        {error && (
          <div className="absolute bottom-4 left-4 right-4 z-10">
            <Alert variant="destructive">
              <AlertTitle>提示</AlertTitle>
              <AlertDescription>{error}</AlertDescription>
            </Alert>
          </div>
        )}
      </div>
    </ThemeProvider>

  );
}

export default App;