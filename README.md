# Log4NetZipAppender
Allows to zip log files when rotating.

Based on [this feature request and associated source](https://issues.apache.org/jira/browse/LOG4NET-579), and changed it a bit to create ZIP files instead of GZ.

To use, just build on Release mode and put the Log4NetAppenderMisc.* files on your log4net-enabled software, and then change your log4net.config file to add an appender like:

	<appender name="RfAppender" type="Log4Net.Appender.Misc.CompressorRollingFileAppender, Log4NetAppenderMisc">
		<file type="log4net.Util.PatternString" value="Logs/integracionCompress.log" />
		<appendToFile value="true" />
		<rollingStyle value="Date" />
		<datePattern value=".yyyyMMddHHmm" />
		<layout type="log4net.Layout.PatternLayout">
			<conversionPattern value="%date (%logger) [%-5level] {%thread} - %message %exception %newline" />
		</layout>
	</appender>

This was tested with latest log4net (2.0.15), change log4net version when building to match your software's version.
