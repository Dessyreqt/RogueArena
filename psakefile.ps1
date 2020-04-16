properties {
    $base_dir = resolve-path .
    $solution_name = "RogueArena"
    $solution_dir = "$base_dir"
    $solution_path = "$solution_dir\$solution_name.sln"
	$runtime_id = "win-x64"
    $project_dir = "$solution_dir\$solution_name"

    $backend_test_dir = "$solution_dir\$solution_name.Tests"
    $test_exe_dir = "$backend_test_dir\bin\Debug\netcoreapp3.1"
    $test_exe_path = "$test_exe_dir\$solution_name.Tests.exe"
    $dotcover_dir = "$base_dir\tools\dotCover"
    $dotcover_exe_path = "$dotcover_dir\dotCover.exe"
    $coverage_report_html_path = "$base_dir\CoverageReports\CoverageReport.html"
    $coverage_report_xml_path = "$base_dir\CoverageReports\CoverageReport.xml"
    $coverage_percent_minimum = 0
    $coverage_filters = "+:type=$solution_name.Commands.*;+:type=$solution_name.Data.*;+:type=$solution_name.Events.*;+:type=$solution_name.Map.*;+:type=$solution_name.Messages.*;-:module=$solution_name.Data.Lookup"
}

task default -depends Clean, Compile, CICoverageReport
task test -depends RunBackEndTests
task coverage -depends CoverageReport

task Clean {
    exec { & dotnet clean $solution_path }
}

task Compile {
    exec { & dotnet build $solution_path }
}

task RunBackEndTests {
    Push-Location -Path $backend_test_dir
    exec { & dotnet fixie --no-build } 
    Pop-Location
}

task CoverageReport {
    Push-Location -Path $backend_test_dir
    exec { & $dotcover_exe_path cover /TargetExecutable=$test_exe_path /Output="$coverage_report_html_path" /ReportType="HTML" /Filters="$coverage_filters" } 
    exec { & $coverage_report_html_path }
    Pop-Location
}

task CICoverageReport {
    Push-Location -Path $test_exe_dir
    exec { & $dotcover_exe_path cover /TargetExecutable=$test_exe_path /Output="$coverage_report_xml_path" /ReportType="XML" /Filters="$coverage_filters" } 
    $xml = [xml](Get-Content $coverage_report_xml_path)
    $coveragePercent = [int](($xml.Root | Select-Object CoveragePercent).CoveragePercent)
    if ($coveragePercent -lt $coverage_percent_minimum) {
        throw "Code coverage is $coveragePercent%. Minimum allowed is $coverage_percent_minimum%."
    }
    Write-Host "$coveragePercent% code coverage!"
    Pop-Location
}

task Publish {
	exec { & dotnet clean -c Release -r $runtime_id $project_dir }
	exec { & dotnet publish -c Release -r $runtime_id $project_dir /p:SelfContained=false }
}
