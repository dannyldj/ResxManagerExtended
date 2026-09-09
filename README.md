# ResxManager EX

여러 폴더에 흩어진 `.resx` 리소스 파일을 **하나의 표로 모아 보고, 편집하고, CSV로 주고받는** 다국어 리소스 관리 도구입니다.

리소스 담당자가 언어별 `.resx` 파일을 하나씩 열어 대조하던 작업을 없애는 것을 목표로, **Windows 데스크톱 앱**과 **브라우저에서 바로 쓰는 웹 앱**을 하나의 Blazor 코드베이스로 함께 제공합니다.

<table>
  <tr>
    <td><b>Live Demo</b></td>
    <td><a href="https://dannyldj.github.io/ResxManagerExtended/">dannyldj.github.io/ResxManagerExtended</a></td>
  </tr>
  <tr>
    <td><b>Download</b></td>
    <td><a href="https://github.com/dannyldj/ResxManagerExtended/releases">Releases</a> (Windows 포터블 단일 실행 파일)</td>
  </tr>
</table>

> 웹 버전은 File System Access API에 의존하므로 Chromium 계열 브라우저에서만 동작합니다. 대용량 리소스에는 데스크톱 버전을 권장합니다.

## 스크린샷

### 리소스 목록 및 편집

![리소스 매니저 화면](https://github.com/user-attachments/assets/e26f7190-b8d4-45b7-9173-c7b98a125470)

폴더 트리에서 경로를 선택하면 하위의 모든 `.resx`를 스캔해 키 × 언어 매트릭스로 보여줍니다.

### 리소스 편집 다이얼로그

![편집 다이얼로그](https://github.com/user-attachments/assets/f4ea7c07-2437-4ff0-9fe2-fa4dc4f1a588)

행을 더블 클릭해 언어별 값을 수정하거나 키를 삭제합니다.

### 설정

![설정 화면](https://github.com/user-attachments/assets/c5fa847d-268b-45fa-bc55-7bf154e9951a)

리소스 루트 경로, 파일명 인식 정규식, UI 언어를 설정합니다.

## 주요 기능

- **디렉터리 일괄 스캔** — 루트 폴더 하나만 지정하면 하위 `.resx`를 모두 찾아 트리 + 데이터 그리드로 표시
- **키 × 언어 매트릭스 뷰** — 중립 리소스와 각 문화권 값을 한 화면에서 나란히 비교, 누락된 번역을 즉시 확인
- **가상화 그리드** — 수만 건 규모의 리소스도 스크롤 지연 없이 렌더링 (`Virtualize`)
- **값 편집 / 일괄 삭제** — 다이얼로그에서 언어별 값 수정, 다중 선택 후 키 일괄 삭제
- **검색 및 정렬** — 키·경로·주석·값 전체 검색, 모든 컬럼 정렬
- **CSV Export / Import** — 번역 담당자에게 CSV로 넘기고, 번역된 CSV를 그대로 되돌려 `.resx`에 반영
- **파일명 규칙 커스터마이즈** — 정규식으로 `Foo.ko.resx` 형태의 문화권 코드 추출 규칙을 직접 지정

## 기술 스택

| 영역 | 사용 기술 |
| --- | --- |
| 런타임 | .NET 10 |
| UI | Blazor, [Fluent UI Blazor](https://www.fluentui-blazor.net/) 4.x |
| 상태 관리 | [Fluxor](https://github.com/mrpmorris/Fluxor) (Flux/Redux 패턴, Redux DevTools 연동) |
| 데스크톱 | WPF + `BlazorWebView` |
| 웹 | Blazor WebAssembly, PWA, [File System Access API](https://developer.chrome.com/docs/capabilities/web-apis/file-system-access) |
| 데이터 | `XDocument` 기반 `.resx` 파싱, CsvHelper |
| CI/CD | GitHub Actions (GitHub Pages 배포, Release 자동 발행) |

## 프로젝트 구조

```
ResxManagerExtended.Shared/     UI · 상태 · 도메인 로직 (플랫폼 무관)
├── Components/                 Razor 페이지 및 다이얼로그
├── Store/                      Fluxor Action / Reducer / Effect
├── Services/                   IResourceService, ISettingService (추상화)
└── Extensions/                 .resx · CSV 변환

ResxManagerExtended.Desktop/    WPF 셸 + Win32 파일 시스템 구현
ResxManagerExtended.Web/        Blazor WASM 셸 + File System Access API 구현
```

플랫폼 의존적인 **파일 접근과 설정 저장만 인터페이스로 분리**했기 때문에, 화면과 비즈니스 로직 전체를 데스크톱과 웹이 그대로 공유합니다.

| 인터페이스 | Desktop | Web |
| --- | --- | --- |
| `IResourceService` | `OpenFolderDialog` / `System.IO` | File System Access API |
| `ISettingService` | `Properties.Settings` | Blazored.LocalStorage |

## 실행 방법

**요구 사항**: [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/dannyldj/ResxManagerExtended.git
cd ResxManagerExtended

# 데스크톱 (Windows)
dotnet run --project ResxManagerExtended.Desktop

# 웹
dotnet run --project ResxManagerExtended.Web
```

## 사용법

1. **Settings** 에서 리소스 루트 디렉터리를 선택합니다.
2. **ResxManager** 에서 트리의 폴더나 파일을 선택하면 리소스가 표로 로드됩니다.
3. 행을 더블 클릭해 값을 편집하거나, 체크박스로 선택해 일괄 삭제합니다.
4. **Export** 로 CSV를 내보내 번역을 진행하고, **Import** 로 번역 결과를 되돌려 반영합니다.

## 문의

버그 리포트와 개선 제안은 [Issues](https://github.com/dannyldj/ResxManagerExtended/issues) 에서 받고 있습니다.
