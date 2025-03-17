import { test, expect } from '@playwright/test'
import { setupTest, cleanupTest } from './test-utils'

test.describe('Location Management', () => {
  test.beforeEach(async ({ page }) => {
    await setupTest(page)
  })

  test.afterEach(async ({ page }) => {
    await cleanupTest(page)
  })

  test('should display locations list', async ({ page }) => {
    await page.goto('/locations')
    await expect(page.getByRole('heading', { name: '拠点管理' })).toBeVisible()
    await expect(page.getByRole('table')).toBeVisible()
  })

  test('should create new location', async ({ page }) => {
    await page.goto('/locations')
    await page.getByRole('button', { name: '新規拠点追加' }).click()
    
    const dialog = page.getByRole('dialog')
    await expect(dialog).toBeVisible()
    
    await dialog.getByLabel('拠点コード').fill('LOC001')
    await dialog.getByLabel('拠点名').fill('テスト拠点')
    await dialog.getByRole('button', { name: '保存' }).click()
    
    await expect(page.getByText('LOC001')).toBeVisible()
    await expect(page.getByText('テスト拠点')).toBeVisible()
  })

  test('should edit location', async ({ page }) => {
    await page.goto('/locations')
    
    // 編集ボタンをクリック
    await page.getByRole('row').filter({ hasText: 'LOC001' })
      .getByRole('button').filter({ hasText: '編集' }).click()
    
    const dialog = page.getByRole('dialog')
    await expect(dialog).toBeVisible()
    
    await dialog.getByLabel('拠点名').fill('更新後拠点')
    await dialog.getByRole('button', { name: '保存' }).click()
    
    await expect(page.getByText('更新後拠点')).toBeVisible()
  })

  test('should delete location', async ({ page }) => {
    await page.goto('/locations')
    
    // 削除ボタンをクリック
    await page.getByRole('row').filter({ hasText: 'LOC001' })
      .getByRole('button').filter({ hasText: '削除' }).click()
    
    const dialog = page.getByRole('dialog')
    await expect(dialog).toBeVisible()
    await dialog.getByRole('button', { name: '削除' }).click()
    
    await expect(page.getByText('LOC001')).not.toBeVisible()
  })

  test('should validate duplicate location code', async ({ page }) => {
    await page.goto('/locations')
    await page.getByRole('button', { name: '新規拠点追加' }).click()
    
    const dialog = page.getByRole('dialog')
    await dialog.getByLabel('拠点コード').fill('LOC001')
    await dialog.getByLabel('拠点名').fill('重複拠点')
    await dialog.getByRole('button', { name: '保存' }).click()
    
    await expect(page.getByText('この拠点コードは既に使用されています')).toBeVisible()
  })

  test('should validate required fields', async ({ page }) => {
    await page.goto('/locations')
    await page.getByRole('button', { name: '新規拠点追加' }).click()
    
    const dialog = page.getByRole('dialog')
    await dialog.getByRole('button', { name: '保存' }).click()
    
    await expect(dialog.getByText('拠点コードは必須です')).toBeVisible()
    await expect(dialog.getByText('拠点名は必須です')).toBeVisible()
  })
}) 