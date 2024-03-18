import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW
import com.kms.katalon.core.testobject.TestObject
import com.kms.katalon.core.util.KeywordUtil
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows

import groovy.console.ui.Console
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser('https://localhost:4200/signin')

WebUI.waitForPageLoad(30)

WebUI.setText(findTestObject('Authentication/Page_Login/input_email'), 'bookingbuddy.landlord@bookingbuddy.com')

WebUI.setEncryptedText(findTestObject('Authentication/Page_Login/input_password'), 'CecxTDGnl/aOPUEZOhmOjg==')

WebUI.click(findTestObject('Authentication/Page_Login/button_SignIn'))

boolean present = WebUI.waitForElementPresent(findTestObject('Page_Home/div_homepage'), 30)

if(!present) {
	KeywordUtil.markFailed("Element 'div_homepage' was not present within the specified timeout.")
}

WebUI.click(findTestObject('Page_Home/a_navMenu'))

boolean visible = WebUI.waitForElementVisible(findTestObject('Page_Home/a_profile'), 30)

if(!visible) {
	KeywordUtil.markFailed("Element 'a_navMenu' was not visible within the specified timeout.")
}

String profileLinkText = WebUI.getText(findTestObject('Page_Home/a_profile'))

if(!profileLinkText.equals('landlord')) {
	KeywordUtil.markFailed("Element 'a_profile' inner text does not match the expected value.")
}

WebUI.closeBrowser()