import { test, expect } from '@playwright/test'
import { initializeTestData } from './test-utils'

test.describe('OS種類管理画面', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/')
    await page.getByRole('link', { name: 'OS種類管理' }).click()
  })

  test('新規OS種類追加', async ({ page }) => {
    await page.getByRole('button', { name: '新規OS種類追加' }).click()
    await page.getByLabel('OS種類名').fill('Windows 12')
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: 'Windows 12' })).toBeVisible()
  })

  test('OS種類編集', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/os-types')
    await page.getByRole('row', { name: 'Windows 10' }).getByRole('button', { name: '編集' }).click()
    await page.getByLabel('OS種類名').fill('Windows 10 Pro')
    await page.getByRole('button', { name: '保存' }).click()
    await expect(page.getByRole('row', { name: 'Windows 10 Pro' })).toBeVisible()
  })

  test('OS種類削除', async ({ page }) => {
    await initializeTestData(page)
    await page.goto('/os-types')
    await page.getByRole('row', { name: 'Windows 10' }).getByRole('button', { name: '削除' }).click()
    await page.getByRole('button', { name: '削除' }).click()
    await expect(page.getByRole('row', { name: 'Windows 10' })).not.toBeVisible()
  })

  test('表題クリックでトップに戻る', async ({ page }) => {
    await page.getByRole('heading', { name: 'OS種類管理' }).click()
    await expect(page).toHaveURL('/')
  })
}) 